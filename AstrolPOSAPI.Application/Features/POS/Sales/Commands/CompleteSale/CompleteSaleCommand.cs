using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AstrolPOSAPI.Domain.Entities.POS;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ItemEntity = AstrolPOSAPI.Domain.Entities.POS.Item;

namespace AstrolPOSAPI.Application.Features.POS.Sales.Commands.CompleteSale
{
    public class CompleteSaleCommand : IRequest<ReceiptDto>
    {
        public string SalesOrderId { get; set; } = default!;
        public bool PrintReceipt { get; set; } = true;
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CompleteSaleCommandValidator : AbstractValidator<CompleteSaleCommand>
    {
        public CompleteSaleCommandValidator()
        {
            RuleFor(p => p.SalesOrderId).NotEmpty();
        }
    }

    public class CompleteSaleCommandHandler : IRequestHandler<CompleteSaleCommand, ReceiptDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompleteSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReceiptDto> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
        {
            // Get the sales order with payments and lines
            var salesOrder = await _unitOfWork.Repository<SalesOrder>().Entities
                .Include(o => o.Payments)
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == request.SalesOrderId, cancellationToken);

            if (salesOrder == null)
                throw new KeyNotFoundException($"Sales order with ID {request.SalesOrderId} not found");

            if (salesOrder.Status != SalesOrderStatus.Completed && salesOrder.Status != SalesOrderStatus.Pending)
                // Allow re-posting if needed or restricted to pending
                if (salesOrder.Status != SalesOrderStatus.Pending)
                    throw new InvalidOperationException("Sales order is not in pending status");

            // Verify payment is complete
            if (salesOrder.AmountPaid < salesOrder.TotalAmount)
                throw new InvalidOperationException($"Insufficient payment. Total: {salesOrder.TotalAmount:C}, Paid: {salesOrder.AmountPaid:C}");

            if (!salesOrder.Lines.Any())
                throw new InvalidOperationException("Cannot complete sale with no items");

            // Start Transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Look up Gen Bus Posting Group (Customer or Default)
                string genBusPostingGroup = "RETAIL"; // Default
                Customer? customer = null;
                if (!string.IsNullOrEmpty(salesOrder.CustomerId))
                {
                    customer = await _unitOfWork.Repository<Customer>().Entities
                        .Include(c => c.GenBusPostingGroup)
                        .Include(c => c.CustomerPostingGroup)
                        .FirstOrDefaultAsync(c => c.Id == salesOrder.CustomerId, cancellationToken);

                    if (customer?.GenBusPostingGroup != null)
                        genBusPostingGroup = customer.GenBusPostingGroup.Code;
                }

                // 2. Process Lines (Inventory + GL)
                foreach (var line in salesOrder.Lines)
                {
                    var item = await _unitOfWork.Repository<ItemEntity>().Entities
                        .Include(i => i.GenProdPostingGroup)
                        .FirstOrDefaultAsync(i => i.Id == line.ItemId, cancellationToken);

                    if (item == null) continue;

                    // Deduct Inventory
                    item.QuantityOnHand -= line.Quantity;
                    if (item.QuantityOnHand < 0) item.QuantityOnHand = 0;
                    await _unitOfWork.Repository<ItemEntity>().UpdateAsync(item);

                    // Financial Posting for line
                    string genProdPostingGroup = item.GenProdPostingGroup?.Code ?? "RETAIL";

                    var postingSetup = await _unitOfWork.Repository<GeneralPostingSetup>().Entities
                        .FirstOrDefaultAsync(ps => ps.GenBusPostingGroupCode == genBusPostingGroup &&
                                                 ps.GenProdPostingGroupCode == genProdPostingGroup &&
                                                 ps.CompanyId == salesOrder.CompanyId, cancellationToken);

                    if (postingSetup != null)
                    {
                        // Credit: Sales Revenue
                        if (!string.IsNullOrEmpty(postingSetup.SalesAccountCode))
                        {
                            await CreateGLEntry(salesOrder, postingSetup.SalesAccountCode, -line.LineTotal, $"Sale {salesOrder.OrderNo} - {item.Name}", cancellationToken);
                        }

                        // Credit: Inventory (Reduction)
                        if (!string.IsNullOrEmpty(postingSetup.InventoryAccountCode))
                        {
                            await CreateGLEntry(salesOrder, postingSetup.InventoryAccountCode, -(item.CostPrice * line.Quantity), $"Inventory reduction {salesOrder.OrderNo} - {item.Name}", cancellationToken);
                        }

                        // Debit: COGS
                        if (!string.IsNullOrEmpty(postingSetup.COGSAccountCode))
                        {
                            await CreateGLEntry(salesOrder, postingSetup.COGSAccountCode, (item.CostPrice * line.Quantity), $"COGS {salesOrder.OrderNo} - {item.Name}", cancellationToken);
                        }
                    }
                }

                // 3. Process Payments (Bank Ledger + GL)
                foreach (var payment in salesOrder.Payments.Where(p => p.Status == PaymentStatus.Completed))
                {
                    if (payment.PaymentMethod == PaymentMethod.CreditAccount && customer != null)
                    {
                        // Debit: Accounts Receivable
                        if (customer.CustomerPostingGroup != null && !string.IsNullOrEmpty(customer.CustomerPostingGroup.ReceivablesAccountCode))
                        {
                            await CreateGLEntry(salesOrder, customer.CustomerPostingGroup.ReceivablesAccountCode, payment.Amount, $"Credit Sale {salesOrder.OrderNo}", cancellationToken);
                        }

                        // Customer Ledger Entry
                        var custEntry = new CustomerLedgerEntry
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerNo = customer.No,
                            PostingDate = DateTime.UtcNow,
                            DocumentNo = salesOrder.OrderNo,
                            Description = $"Sale {salesOrder.OrderNo}",
                            Amount = payment.Amount,
                            RemainingAmount = payment.Amount,
                            Open = true,
                            CompanyId = salesOrder.CompanyId
                        };
                        await _unitOfWork.Repository<CustomerLedgerEntry>().AddAsync(custEntry);
                    }
                    else
                    {
                        // Debit: Bank/Cash Account
                        var bankAccount = await _unitOfWork.Repository<BankAccount>().Entities
                            .FirstOrDefaultAsync(b => b.AccountType == (BankAccountType)payment.PaymentMethod && b.CompanyId == salesOrder.CompanyId, cancellationToken);

                        // Fallback: search by name or first available of type
                        if (bankAccount == null)
                        {
                            bankAccount = await _unitOfWork.Repository<BankAccount>().Entities
                                .FirstOrDefaultAsync(b => b.CompanyId == salesOrder.CompanyId, cancellationToken);
                        }

                        if (bankAccount != null && !string.IsNullOrEmpty(bankAccount.GLAccountCode))
                        {
                            await CreateGLEntry(salesOrder, bankAccount.GLAccountCode, payment.Amount, $"Payment {salesOrder.OrderNo} via {payment.PaymentMethod}", cancellationToken);

                            // Bank Ledger Entry
                            var bankEntry = new BankLedgerEntry
                            {
                                Id = Guid.NewGuid().ToString(),
                                BankAccountCode = bankAccount.Code,
                                PostingDate = DateTime.UtcNow,
                                DocumentNo = salesOrder.OrderNo,
                                Description = $"Payment {salesOrder.OrderNo} via {payment.PaymentMethod}",
                                Amount = payment.Amount,
                                CompanyId = salesOrder.CompanyId
                            };
                            await _unitOfWork.Repository<BankLedgerEntry>().AddAsync(bankEntry);

                            // Update Bank Balance
                            bankAccount.Balance += payment.Amount;
                            await _unitOfWork.Repository<BankAccount>().UpdateAsync(bankAccount);
                        }
                    }
                }

                // Update sales order status
                salesOrder.Status = SalesOrderStatus.Completed;
                await _unitOfWork.Repository<SalesOrder>().UpdateAsync(salesOrder);

                // Generate receipt
                var receiptNo = $"RCP-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
                var receipt = new Receipt
                {
                    SalesOrderId = request.SalesOrderId,
                    ReceiptNo = receiptNo,
                    IssuedDate = DateTime.UtcNow,
                    TotalAmount = salesOrder.TotalAmount,
                    AmountPaid = salesOrder.AmountPaid,
                    ChangeGiven = salesOrder.ChangeGiven,
                    IsPrinted = request.PrintReceipt,
                    IsSentElectronically = !string.IsNullOrEmpty(request.EmailAddress) || !string.IsNullOrEmpty(request.PhoneNumber),
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber
                };

                await _unitOfWork.Repository<Receipt>().AddAsync(receipt);
                await _unitOfWork.Save(cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<ReceiptDto>(receipt);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task CreateGLEntry(SalesOrder order, string accountNo, decimal amount, string description, CancellationToken cancellationToken)
        {
            var entry = new GLEntry
            {
                Id = Guid.NewGuid().ToString(),
                PostingDate = DateTime.UtcNow,
                DocumentNo = order.OrderNo,
                GLAccountNo = accountNo,
                Description = description,
                Amount = amount,
                DebitAmount = amount > 0 ? amount : 0,
                CreditAmount = amount < 0 ? -amount : 0,
                SourceNo = order.CustomerId ?? "CASH",
                CompanyId = order.CompanyId
            };
            await _unitOfWork.Repository<GLEntry>().AddAsync(entry);
        }
    }
}

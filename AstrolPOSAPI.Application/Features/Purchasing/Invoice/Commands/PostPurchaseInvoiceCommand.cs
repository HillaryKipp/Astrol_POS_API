using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AstrolPOSAPI.Domain.Entities.Purchasing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.Commands
{
    public class PostPurchaseInvoiceCommand : IRequest<string>
    {
        public string PurchaseHeaderId { get; set; } = string.Empty;
    }

    public class PostPurchaseInvoiceCommandHandler : IRequestHandler<PostPurchaseInvoiceCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INoSeriesService _noSeriesService;

        public PostPurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork, INoSeriesService noSeriesService)
        {
            _unitOfWork = unitOfWork;
            _noSeriesService = noSeriesService;
        }

        public async Task<string> Handle(PostPurchaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the unposted document
            var header = await _unitOfWork.Repository<PurchaseHeader>().Entities
                .Include(h => h.Lines)
                .FirstOrDefaultAsync(h => h.Id == request.PurchaseHeaderId && h.Status != PurchaseStatus.Posted, cancellationToken);

            if (header == null)
                throw new KeyNotFoundException("Purchase Invoice not found or already posted.");

            if (!header.Lines.Any())
                throw new InvalidOperationException("Cannot post an invoice with no lines.");

            // 2. Fetch Vendor and Posting Groups
            var vendor = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>().Entities
                .Include(v => v.VendorPostingGroup)
                .FirstOrDefaultAsync(v => v.No == header.BuyFromVendorNo && v.CompanyId == header.CompanyId, cancellationToken);

            if (vendor == null)
                throw new InvalidOperationException($"Vendor {header.BuyFromVendorNo} not found.");

            if (vendor.VendorPostingGroup == null || string.IsNullOrEmpty(vendor.VendorPostingGroup.PayablesAccountCode))
                throw new InvalidOperationException($"Vendor {header.BuyFromVendorNo} does not have a valid Posting Group setup.");

            // 3. Start Transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 4. Generate Posted Invoice No
                var postedNo = await _noSeriesService.GenerateNextNumberAsync("POSTED_PURCH_INV", cancellationToken);

                // 5. Create Posted Invoice (History)
                var postedHeader = new PurchInvHeader
                {
                    Id = Guid.NewGuid().ToString(),
                    No = postedNo,
                    BuyFromVendorNo = header.BuyFromVendorNo,
                    BuyFromVendorName = header.BuyFromVendorName,
                    PostingDate = header.PostingDate,
                    DocumentDate = header.DocumentDate,
                    DueDate = header.DueDate,
                    VendorInvoiceNo = header.VendorInvoiceNo,
                    AttachmentUrl = header.AttachmentUrl,
                    SourceNo = header.No,
                    CompanyId = header.CompanyId
                };

                decimal totalAmount = 0;
                foreach (var line in header.Lines)
                {
                    postedHeader.Lines.Add(new PurchInvLine
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentNo = postedNo,
                        ItemNo = line.ItemNo,
                        Description = line.Description,
                        Quantity = line.Quantity,
                        DirectUnitCost = line.DirectUnitCost,
                        LineAmount = line.LineAmount,
                        CompanyId = header.CompanyId
                    });
                    totalAmount += line.LineAmount;
                }

                await _unitOfWork.Repository<PurchInvHeader>().AddAsync(postedHeader);

                // 6. Post to General Ledger (Double Entry)
                // Credit: Accounts Payable
                await CreateGLEntry(postedHeader, vendor.VendorPostingGroup.PayablesAccountCode!, -totalAmount, cancellationToken);

                // Debit: Expense/Inventory (Simplified: using ServiceChargeAccount from Posting Group for all lines for now)
                // In a full implementation, this would look up the posting account per item/line.
                var expenseAccount = vendor.VendorPostingGroup.ServiceChargeAccountCode ?? "6000"; // Fallback to a default if not set
                await CreateGLEntry(postedHeader, expenseAccount, totalAmount, cancellationToken);

                // 7. Post to Vendor Ledger
                var vendorEntry = new VendorLedgerEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    VendorNo = header.BuyFromVendorNo,
                    PostingDate = header.PostingDate,
                    DocumentNo = postedNo,
                    Description = $"Purchase Invoice {header.VendorInvoiceNo}",
                    Amount = -totalAmount, // Credit to vendor is negative in BC style
                    RemainingAmount = -totalAmount,
                    Open = true,
                    ExternalDocumentNo = header.VendorInvoiceNo,
                    CompanyId = header.CompanyId
                };
                await _unitOfWork.Repository<VendorLedgerEntry>().AddAsync(vendorEntry);

                // 8. Delete Unposted Document
                await _unitOfWork.Repository<PurchaseHeader>().DeleteAsync(header);

                // 9. Save and Commit
                await _unitOfWork.Save(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return postedNo;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task CreateGLEntry(PurchInvHeader header, string accountNo, decimal amount, CancellationToken cancellationToken)
        {
            var entry = new GLEntry
            {
                Id = Guid.NewGuid().ToString(),
                PostingDate = header.PostingDate,
                DocumentNo = header.No,
                GLAccountNo = accountNo,
                Description = $"Purchase Invoice {header.VendorInvoiceNo}",
                Amount = amount,
                DebitAmount = amount > 0 ? amount : 0,
                CreditAmount = amount < 0 ? -amount : 0,
                SourceNo = header.BuyFromVendorNo,
                ExternalDocumentNo = header.VendorInvoiceNo,
                CompanyId = header.CompanyId
            };
            await _unitOfWork.Repository<GLEntry>().AddAsync(entry);
        }
    }
}

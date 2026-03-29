using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Commands
{
    /// <summary>
    /// Posts a Payment Voucher: creates double-entry GL entries,
    /// Vendor Ledger Entry, Bank Ledger Entry, and updates Bank balance.
    /// </summary>
    public class PostPaymentVoucherCommand : IRequest<string>
    {
        public string PaymentVoucherId { get; set; } = string.Empty;
    }

    public class PostPaymentVoucherCommandHandler : IRequestHandler<PostPaymentVoucherCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostPaymentVoucherCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(PostPaymentVoucherCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch PV
            var pv = await _unitOfWork.Repository<Domain.Entities.Purchasing.PaymentVoucher>().Entities
                .FirstOrDefaultAsync(p => p.Id == request.PaymentVoucherId && p.Status == Domain.Entities.Purchasing.PaymentVoucherStatus.Open, cancellationToken);

            if (pv == null)
                throw new KeyNotFoundException("Payment Voucher not found or already posted.");

            // 2. Fetch Vendor with Posting Group
            var vendor = await _unitOfWork.Repository<Domain.Entities.Purchasing.Vendor>().Entities
                .Include(v => v.VendorPostingGroup)
                .FirstOrDefaultAsync(v => v.No == pv.VendorNo && v.CompanyId == pv.CompanyId, cancellationToken);

            if (vendor?.VendorPostingGroup == null || string.IsNullOrEmpty(vendor.VendorPostingGroup.PayablesAccountCode))
                throw new InvalidOperationException($"Vendor '{pv.VendorNo}' does not have a valid Posting Group with a Payables Account.");

            // 3. Fetch Bank Account
            var bank = await _unitOfWork.Repository<BankAccount>().Entities
                .FirstOrDefaultAsync(b => b.Code == pv.BankAccountCode && b.CompanyId == pv.CompanyId, cancellationToken);

            if (bank == null || string.IsNullOrEmpty(bank.GLAccountCode))
                throw new InvalidOperationException($"Bank Account '{pv.BankAccountCode}' not found or missing GL Account mapping.");

            // 4. Begin Transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 5. GL Entry: DEBIT Vendor Payables (reduces liability)
                await CreateGLEntry(pv, vendor.VendorPostingGroup.PayablesAccountCode!, pv.Amount, cancellationToken);

                // 6. GL Entry: CREDIT Bank Account
                await CreateGLEntry(pv, bank.GLAccountCode!, -pv.Amount, cancellationToken);

                // 7. Vendor Ledger Entry (positive = payment reducing balance)
                var vle = new VendorLedgerEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    VendorNo = pv.VendorNo,
                    PostingDate = pv.PostingDate,
                    DocumentNo = pv.No,
                    Description = $"Payment Voucher {pv.No}",
                    Amount = pv.Amount,
                    RemainingAmount = pv.Amount,
                    Open = true,
                    CompanyId = pv.CompanyId
                };
                await _unitOfWork.Repository<VendorLedgerEntry>().AddAsync(vle);

                // 8. Bank Ledger Entry
                var ble = new BankLedgerEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    BankAccountCode = pv.BankAccountCode,
                    PostingDate = pv.PostingDate,
                    DocumentNo = pv.No,
                    Description = $"Payment to {pv.VendorName}",
                    Amount = -pv.Amount,
                    CompanyId = pv.CompanyId
                };
                await _unitOfWork.Repository<BankLedgerEntry>().AddAsync(ble);

                // 9. Update Bank Balance
                bank.Balance -= pv.Amount;
                await _unitOfWork.Repository<BankAccount>().UpdateAsync(bank);

                // 10. Mark PV as Posted
                pv.Status = Domain.Entities.Purchasing.PaymentVoucherStatus.Posted;
                await _unitOfWork.Repository<Domain.Entities.Purchasing.PaymentVoucher>().UpdateAsync(pv);

                // 11. Save and Commit
                await _unitOfWork.Save(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return pv.No;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task CreateGLEntry(Domain.Entities.Purchasing.PaymentVoucher pv, string accountNo, decimal amount, CancellationToken cancellationToken)
        {
            var entry = new GLEntry
            {
                Id = Guid.NewGuid().ToString(),
                PostingDate = pv.PostingDate,
                DocumentNo = pv.No,
                GLAccountNo = accountNo,
                Description = $"Payment Voucher {pv.No} - {pv.VendorName}",
                Amount = amount,
                DebitAmount = amount > 0 ? amount : 0,
                CreditAmount = amount < 0 ? -amount : 0,
                SourceNo = pv.VendorNo,
                CompanyId = pv.CompanyId
            };
            await _unitOfWork.Repository<GLEntry>().AddAsync(entry);
        }
    }
}

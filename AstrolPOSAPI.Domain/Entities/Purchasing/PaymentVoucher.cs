using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Purchasing
{
    /// <summary>
    /// Payment Voucher for vendor payments.
    /// Debits the vendor payables account and credits the selected bank account.
    /// </summary>
    public class PaymentVoucher : BaseAuditableEntity
    {
        public string No { get; set; } = string.Empty;
        public DateTime PostingDate { get; set; }

        public string VendorNo { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;

        public string BankAccountCode { get; set; } = string.Empty;
        public string BankAccountName { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public PaymentVoucherStatus Status { get; set; } = PaymentVoucherStatus.Open;

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }

    public enum PaymentVoucherStatus
    {
        Open = 0,
        Posted = 1
    }
}

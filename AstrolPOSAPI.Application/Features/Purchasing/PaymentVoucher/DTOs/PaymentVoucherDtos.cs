namespace AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs
{
    public class PaymentVoucherDto
    {
        public string Id { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public DateTime PostingDate { get; set; }
        public string VendorNo { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string BankAccountCode { get; set; } = string.Empty;
        public string BankAccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreatePaymentVoucherDto
    {
        public DateTime PostingDate { get; set; }
        public string VendorNo { get; set; } = string.Empty;
        public string BankAccountCode { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }
}

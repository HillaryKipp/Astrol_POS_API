using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Purchasing
{
    public class PurchaseHeader : BaseAuditableEntity
    {
        public string No { get; set; } = string.Empty;
        public string BuyFromVendorNo { get; set; } = string.Empty;
        public string BuyFromVendorName { get; set; } = string.Empty;

        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }

        public string? VendorInvoiceNo { get; set; }
        public string? AttachmentUrl { get; set; }
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Open;

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public ICollection<PurchaseLine> Lines { get; set; } = new List<PurchaseLine>();
    }

    public enum PurchaseStatus
    {
        Open = 0,
        Released = 1,
        Posted = 2
    }
}

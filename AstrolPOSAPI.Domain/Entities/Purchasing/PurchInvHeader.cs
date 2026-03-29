using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Purchasing
{
    public class PurchInvHeader : BaseAuditableEntity
    {
        public string No { get; set; } = string.Empty;
        public string BuyFromVendorNo { get; set; } = string.Empty;
        public string BuyFromVendorName { get; set; } = string.Empty;

        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }

        public string? VendorInvoiceNo { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? SourceNo { get; set; } // Reference to original PurchaseHeader No

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public ICollection<PurchInvLine> Lines { get; set; } = new List<PurchInvLine>();
    }
}

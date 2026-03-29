using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class VendorLedgerEntry : BaseAuditableEntity
    {
        public long EntryNo { get; set; }
        public string VendorNo { get; set; } = string.Empty;
        public DateTime PostingDate { get; set; }
        public string DocumentNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool Open { get; set; } = true;

        public string? ExternalDocumentNo { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

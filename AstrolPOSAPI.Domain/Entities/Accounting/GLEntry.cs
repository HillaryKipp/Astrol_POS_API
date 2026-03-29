using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class GLEntry : BaseAuditableEntity
    {
        public long EntryNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string DocumentNo { get; set; } = string.Empty;
        public string GLAccountNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }

        public string? SourceNo { get; set; } // Vendor No, Customer No, etc.
        public string? ExternalDocumentNo { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

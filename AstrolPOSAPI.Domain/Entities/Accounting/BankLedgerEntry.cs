using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class BankLedgerEntry : BaseAuditableEntity
    {
        public long EntryNo { get; set; }
        public string BankAccountCode { get; set; } = string.Empty;
        public DateTime PostingDate { get; set; }
        public string DocumentNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

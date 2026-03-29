using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class CustomerPostingGroup : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // GL Account for Accounts Receivable
        public string? ReceivablesAccountCode { get; set; }

        // Default Sales Account for this group
        public string? SalesAccountCode { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

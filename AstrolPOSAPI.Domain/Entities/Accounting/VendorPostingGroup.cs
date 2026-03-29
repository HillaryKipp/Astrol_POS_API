using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class VendorPostingGroup : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // GL Account for Accounts Payable
        public string? PayablesAccountCode { get; set; }

        // GL Account for Service/Expense that doesn't go to inventory
        public string? ServiceChargeAccountCode { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

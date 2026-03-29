using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class Customer : BaseAuditableEntity
    {
        public string No { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? CustomerPostingGroupId { get; set; }
        public CustomerPostingGroup? CustomerPostingGroup { get; set; }

        public string? GenBusPostingGroupId { get; set; }
        public GenBusPostingGroup? GenBusPostingGroup { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Accounting;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Purchasing
{
    public class Vendor : BaseAuditableEntity
    {
        public string No { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? VATRegistrationNo { get; set; }
        public bool Blocked { get; set; }

        // Posting Groups
        public string? VendorPostingGroupId { get; set; }
        public VendorPostingGroup? VendorPostingGroup { get; set; }

        public string? GenBusPostingGroupId { get; set; }
        public GenBusPostingGroup? GenBusPostingGroup { get; set; }

        // New requested fields
        public string? ImageUrl { get; set; }
        public string? DocumentUrl { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

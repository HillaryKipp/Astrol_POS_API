using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Core
{
    public class Store : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        
        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
        
        public string? StoreTypeId { get; set; }
        public StoreType? StoreType { get; set; }
    }
}

using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Core
{
    public class StoreType : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool HasOtp { get; set; }
        public string? CompanyId { get; set; }
    }
}

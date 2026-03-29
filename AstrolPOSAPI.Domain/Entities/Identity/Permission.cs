using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Identity;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class Permission : BaseAuditableEntity
    {
        public string ResourceName { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? RoleId { get; set; }
        public AppRole? Role { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanWrite { get; set; }
        public bool CanEdit { get; set; }
    }
}

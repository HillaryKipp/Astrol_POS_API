using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;
using Microsoft.AspNetCore.Identity;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string? EmpNo { get; set; }
        public string? NationalID { get; set; }
        public string? CompanyId { get; set; }
        public string? StoreOfOperationId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool PasswordChangeRequired { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public int LoginCount { get; set; }
        public bool IsDeleted { get; set; }
        public string? Role { get; set; }

        // Navigation properties
        public virtual Company? Company { get; set; }
        public virtual Store? StoreOfOperation { get; set; }
        public virtual ICollection<UserStore> UserStores { get; set; } = new HashSet<UserStore>();
    }
}

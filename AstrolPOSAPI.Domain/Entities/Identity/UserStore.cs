using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Domain.Entities.Identity;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class UserStore
    {
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
        
        public string StoreId { get; set; } = string.Empty;
        public Store? Store { get; set; }
        
        public bool IsPrimary { get; set; }
    }
}

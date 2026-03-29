using Microsoft.AspNetCore.Identity;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class AppRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}

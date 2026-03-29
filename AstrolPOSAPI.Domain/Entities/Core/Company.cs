using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Core
{
    public class Company : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Store> Stores { get; set; } = new List<Store>();
    }
}

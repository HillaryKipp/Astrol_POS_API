using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Domain.Entities.Identity;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    public class AssignedDrawer : BaseAuditableEntity
    {
        public string DrawerId { get; set; } = string.Empty;
        public Drawer? Drawer { get; set; }

        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        public decimal OpenCash { get; set; }
        public DateTime AssignedAt { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }

        public string? DefaultScreenId { get; set; }
        public DefaultScreen? DefaultScreen { get; set; }

        public string? DefaultShortcutBar { get; set; }
        public DateTimeOffset? SessionTimeIn { get; set; }
        public DateTimeOffset? SessionTimeOut { get; set; }
    }
}

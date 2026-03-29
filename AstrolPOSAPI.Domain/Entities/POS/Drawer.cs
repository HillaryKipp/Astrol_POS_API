using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    public class Drawer : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DrawerStatus Status { get; set; }
        
        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
        
        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }
        
        public string DrawerGroupId { get; set; } = string.Empty;
        public DrawerGroup? DrawerGroup { get; set; }
        
        public string TerminalId { get; set; } = string.Empty;
        public Terminal? Terminal { get; set; }
        
        public string? DefaultScreenId { get; set; }
        public DefaultScreen? DefaultScreen { get; set; }
    }

    public enum DrawerStatus
    {
        Closed = 0,
        Open = 1
    }
}

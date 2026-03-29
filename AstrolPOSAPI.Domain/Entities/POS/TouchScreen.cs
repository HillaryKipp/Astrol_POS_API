using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    public class TouchScreen : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        
        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
        
        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }
        
        public ICollection<TouchScreenButton> Buttons { get; set; } = new List<TouchScreenButton>();
    }
}

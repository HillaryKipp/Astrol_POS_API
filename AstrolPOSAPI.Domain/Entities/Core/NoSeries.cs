using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Core
{
    public class NoSeries : BaseAuditableEntity
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
        
        // Merged from POS version
        public bool Manual { get; set; } = false;
    }
}

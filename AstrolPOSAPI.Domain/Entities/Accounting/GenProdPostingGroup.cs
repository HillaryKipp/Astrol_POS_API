using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class GenProdPostingGroup : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

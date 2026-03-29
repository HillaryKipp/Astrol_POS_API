using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class GeneralPostingSetup : BaseAuditableEntity
    {
        public string GenBusPostingGroupCode { get; set; } = string.Empty;
        public string GenProdPostingGroupCode { get; set; } = string.Empty;

        public string? SalesAccountCode { get; set; }
        public string? PurchaseAccountCode { get; set; }
        public string? COGSAccountCode { get; set; }
        public string? InventoryAccountCode { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

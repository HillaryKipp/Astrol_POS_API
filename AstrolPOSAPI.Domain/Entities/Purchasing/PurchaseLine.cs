using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Purchasing
{
    public class PurchaseLine : BaseAuditableEntity
    {
        public string DocumentNo { get; set; } = string.Empty;
        public string ItemNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal LineAmount { get; set; }

        public string CompanyId { get; set; } = string.Empty;

        public string? PurchaseHeaderId { get; set; }
        public PurchaseHeader? PurchaseHeader { get; set; }
    }
}

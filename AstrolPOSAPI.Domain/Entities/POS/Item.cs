using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a product/item that can be sold via POS
    /// </summary>
    public class Item : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string? CategoryId { get; set; }
        public ItemCategory? Category { get; set; }

        /// <summary>
        /// Unit of measure (e.g., EA, KG, L, etc.)
        /// </summary>
        public string UnitOfMeasure { get; set; } = "EA";

        /// <summary>
        /// Selling price per unit
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Cost price for profit calculation
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// URL or path to item image
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Current stock quantity
        /// </summary>
        public decimal QuantityOnHand { get; set; }

        /// <summary>
        /// Minimum stock level for alerts
        /// </summary>
        public decimal ReorderLevel { get; set; }

        /// <summary>
        /// Tax rate percentage (e.g., 16 for 16%)
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Whether item is available for sale
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Barcode for scanning
        /// </summary>
        public string? Barcode { get; set; }

        public string? GenProdPostingGroupId { get; set; }
        public AstrolPOSAPI.Domain.Entities.Accounting.GenProdPostingGroup? GenProdPostingGroup { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }
    }
}

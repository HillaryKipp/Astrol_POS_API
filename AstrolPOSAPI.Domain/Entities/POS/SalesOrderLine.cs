using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a line item in a sales order
    /// </summary>
    public class SalesOrderLine : BaseAuditableEntity
    {
        public string SalesOrderId { get; set; } = string.Empty;
        public SalesOrder? SalesOrder { get; set; }

        public string ItemId { get; set; } = string.Empty;
        public Item? Item { get; set; }

        /// <summary>
        /// Snapshot of item code at time of sale
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Snapshot of item name at time of sale
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; } = "EA";

        /// <summary>
        /// Quantity sold
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price at time of sale
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to this line
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Tax rate for this item
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Tax amount for this line
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Line total (Quantity * UnitPrice - Discount + Tax)
        /// </summary>
        public decimal LineTotal { get; set; }
    }
}

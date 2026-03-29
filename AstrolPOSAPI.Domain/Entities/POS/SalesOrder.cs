using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Domain.Entities.Identity;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a sales transaction/order
    /// </summary>
    public class SalesOrder : BaseAuditableEntity
    {
        /// <summary>
        /// Auto-generated order number
        /// </summary>
        public string OrderNo { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Pending;

        /// <summary>
        /// Optional customer reference
        /// </summary>
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        /// <summary>
        /// The cashier processing this sale
        /// </summary>
        public string CashierId { get; set; } = string.Empty;
        public AppUser? Cashier { get; set; }

        /// <summary>
        /// The drawer/register used for this sale
        /// </summary>
        public string DrawerId { get; set; } = string.Empty;
        public Drawer? Drawer { get; set; }

        /// <summary>
        /// Subtotal before discounts and taxes
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Total discount applied to the order
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Total tax amount
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Final total (Subtotal - Discount + Tax)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Total amount paid
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Change given to customer
        /// </summary>
        public decimal ChangeGiven { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }

        /// <summary>
        /// Order line items
        /// </summary>
        public ICollection<SalesOrderLine> Lines { get; set; } = new List<SalesOrderLine>();

        /// <summary>
        /// Payments applied to this order
        /// </summary>
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public enum SalesOrderStatus
    {
        /// <summary>
        /// Sale in progress (cart mode)
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Payment completed, inventory deducted
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Sale cancelled/voided
        /// </summary>
        Voided = 2,

        /// <summary>
        /// On hold for later
        /// </summary>
        OnHold = 3
    }
}

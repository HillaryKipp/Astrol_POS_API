using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a receipt generated after a successful sale
    /// </summary>
    public class Receipt : BaseAuditableEntity
    {
        public string SalesOrderId { get; set; } = string.Empty;
        public SalesOrder? SalesOrder { get; set; }

        /// <summary>
        /// Auto-generated receipt number
        /// </summary>
        public string ReceiptNo { get; set; } = string.Empty;

        /// <summary>
        /// When the receipt was issued
        /// </summary>
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Total amount on receipt
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Amount customer paid
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Change returned to customer
        /// </summary>
        public decimal ChangeGiven { get; set; }

        /// <summary>
        /// Whether receipt has been printed
        /// </summary>
        public bool IsPrinted { get; set; }

        /// <summary>
        /// Whether receipt was sent electronically (email/SMS)
        /// </summary>
        public bool IsSentElectronically { get; set; }

        /// <summary>
        /// Email address receipt was sent to
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Phone number receipt was sent to
        /// </summary>
        public string? PhoneNumber { get; set; }
    }
}

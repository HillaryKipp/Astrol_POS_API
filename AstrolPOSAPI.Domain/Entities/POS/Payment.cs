using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a payment transaction for a sales order
    /// </summary>
    public class Payment : BaseAuditableEntity
    {
        public string SalesOrderId { get; set; } = string.Empty;
        public SalesOrder? SalesOrder { get; set; }

        /// <summary>
        /// Payment method used
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Amount paid via this payment method
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Transaction reference (e.g., M-Pesa code, card authorization)
        /// </summary>
        public string? ReferenceNo { get; set; }

        /// <summary>
        /// For M-Pesa: phone number used
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// For card payments: last 4 digits
        /// </summary>
        public string? CardLastFour { get; set; }

        /// <summary>
        /// Payment status
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// When the payment was processed
        /// </summary>
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Response message from payment gateway
        /// </summary>
        public string? ResponseMessage { get; set; }
    }

    public enum PaymentMethod
    {
        Cash = 0,
        Card = 1,
        Mpesa = 2,
        Visa = 3,
        Mastercard = 4,
        BankTransfer = 5,
        CreditAccount = 6
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Cancelled = 3,
        Refunded = 4
    }
}

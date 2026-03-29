using System.ComponentModel.DataAnnotations;
using AstrolPOSAPI.Domain.Entities.POS;

namespace AstrolPOSAPI.Application.Features.POS.Sales.DTOs
{
    public class SalesOrderDto
    {
        public string Id { get; set; } = default!;
        public string OrderNo { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public SalesOrderStatus Status { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string CashierId { get; set; } = default!;
        public string? CashierName { get; set; }
        public string DrawerId { get; set; } = default!;
        public string? DrawerName { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeGiven { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
        public List<SalesOrderLineDto> Lines { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }

    public class SalesOrderLineDto
    {
        public string Id { get; set; } = default!;
        public string SalesOrderId { get; set; } = default!;
        public string ItemId { get; set; } = default!;
        public string ItemCode { get; set; } = default!;
        public string ItemName { get; set; } = default!;
        public string UnitOfMeasure { get; set; } = default!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class PaymentDto
    {
        public string Id { get; set; } = default!;
        public string SalesOrderId { get; set; } = default!;
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CardLastFour { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ResponseMessage { get; set; }
    }

    public class ReceiptDto
    {
        public string Id { get; set; } = default!;
        public string SalesOrderId { get; set; } = default!;
        public string ReceiptNo { get; set; } = default!;
        public DateTime IssuedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeGiven { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsSentElectronically { get; set; }
    }

    // Request DTOs
    public class CreateSalesOrderDto
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        [Required]
        public string CashierId { get; set; } = default!;

        [Required]
        public string DrawerId { get; set; } = default!;

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class AddItemToSaleDto
    {
        [Required]
        public string ItemId { get; set; } = default!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        public decimal? DiscountAmount { get; set; }
    }

    public class ProcessPaymentDto
    {
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// For M-Pesa: phone number to charge
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// For M-Pesa: reference from STK push response
        /// For Card: authorization code
        /// </summary>
        public string? ReferenceNo { get; set; }

        /// <summary>
        /// For card payments: last 4 digits
        /// </summary>
        public string? CardLastFour { get; set; }
    }
}

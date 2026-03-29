using System.ComponentModel.DataAnnotations;

namespace AstrolPOSAPI.Application.Features.POS.Item.DTOs
{
    public class ItemDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string UnitOfMeasure { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string? ImageUrl { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal TaxRate { get; set; }
        public bool IsActive { get; set; }
        public string? Barcode { get; set; }
        public string? GenProdPostingGroupId { get; set; }
        public string? GenProdPostingGroupCode { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateItemDto
    {
        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? CategoryId { get; set; }

        [MaxLength(16)]
        public string UnitOfMeasure { get; set; } = "EA";

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal QuantityOnHand { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ReorderLevel { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;

        public string? GenProdPostingGroupId { get; set; }
    }

    public class UpdateItemDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? CategoryId { get; set; }

        [MaxLength(16)]
        public string UnitOfMeasure { get; set; } = "EA";

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal QuantityOnHand { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ReorderLevel { get; set; }

        [Range(0, 100)]
        public decimal TaxRate { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        public string? GenProdPostingGroupId { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

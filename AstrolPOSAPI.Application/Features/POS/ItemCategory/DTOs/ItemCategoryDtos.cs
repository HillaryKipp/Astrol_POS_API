using System.ComponentModel.DataAnnotations;

namespace AstrolPOSAPI.Application.Features.POS.ItemCategory.DTOs
{
    public class ItemCategoryDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string StoreOfOperationId { get; set; } = default!;
        public string? StoreOfOperationName { get; set; }
    }

    public class CreateItemCategoryDto
    {
        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateItemCategoryDto
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

        public bool IsActive { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

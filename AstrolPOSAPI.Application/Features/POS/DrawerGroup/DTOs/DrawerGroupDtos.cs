using System.ComponentModel.DataAnnotations;

namespace AstrolPOSAPI.Application.Features.POS.DrawerGroup.DTOs
{
    public class DrawerGroupDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string StoreOfOperationId { get; set; } = default!;
        public string? StoreOfOperationName { get; set; }
    }

    public class CreateDrawerGroupDto
    {
        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Description { get; set; } = default!;

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateDrawerGroupDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Description { get; set; } = default!;

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

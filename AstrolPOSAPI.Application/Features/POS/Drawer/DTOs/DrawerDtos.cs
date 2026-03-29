using AstrolPOSAPI.Domain.Entities.POS;
using System.ComponentModel.DataAnnotations;

namespace AstrolPOSAPI.Application.Features.POS.Drawer.DTOs
{
    public class DrawerDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string DrawerGroupId { get; set; } = default!;
        public string? DefaultScreenId { get; set; }
        public string TerminalId { get; set; } = default!;
        public DrawerStatus Status { get; set; }
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string? StoreOfOperationName { get; set; }
        public string? DrawerGroupName { get; set; }
        public string? TerminalName { get; set; }
        public string? DefaultScreenName { get; set; }

        // Optional: include related entity names/codes if needed
    }

    public class CreateDrawerDto
    {
        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [Required]
        public string DrawerGroupId { get; set; } = default!;

        public string? DefaultScreenId { get; set; }

        [Required]
        public string TerminalId { get; set; } = default!;

        [Required]
        public DrawerStatus Status { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateDrawerDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [Required]
        public string DrawerGroupId { get; set; } = default!;

        public string? DefaultScreenId { get; set; }

        [Required]
        public string TerminalId { get; set; } = default!;

        [Required]
        public DrawerStatus Status { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

using AstrolPOSAPI.Domain.Entities.POS;
using System.ComponentModel.DataAnnotations;

namespace AstrolPOSAPI.Application.Features.POS.AssignedDrawer.DTOs
{
    public class AssignedDrawerDto
    {
        public string Id { get; set; } = default!;
        public string DrawerId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string? DefaultScreenId { get; set; }
        public string? DefaultShortcutBar { get; set; }
        public DateTimeOffset? SessionTimeIn { get; set; }
        public DateTimeOffset? SessionTimeOut { get; set; }
        public decimal OpenCash { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string StoreOfOperationId { get; set; } = default!;
        public string? StoreOfOperationName { get; set; }
        public string? DrawerName { get; set; }
        public DrawerStatus? DrawerStatus { get; set; }
        public string? DefaultScreenName { get; set; }
        public string? UserName { get; set; }
    }

    public class CreateAssignedDrawerDto
    {
        [Required]
        public string DrawerId { get; set; } = default!;

        [Required]
        public string UserId { get; set; } = default!;

        public string? DefaultScreenId { get; set; }
        public string? DefaultShortcutBar { get; set; }
        public DateTimeOffset? SessionTimeIn { get; set; }
        public DateTimeOffset? SessionTimeOut { get; set; }

        [Required]
        public decimal OpenCash { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateAssignedDrawerDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        public string DrawerId { get; set; } = default!;

        [Required]
        public string UserId { get; set; } = default!;

        public string? DefaultScreenId { get; set; }
        public string? DefaultShortcutBar { get; set; }
        public DateTimeOffset? SessionTimeIn { get; set; }
        public DateTimeOffset? SessionTimeOut { get; set; }

        [Required]
        public decimal OpenCash { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

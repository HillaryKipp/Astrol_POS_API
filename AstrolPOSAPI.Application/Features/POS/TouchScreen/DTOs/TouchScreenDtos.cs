using System.ComponentModel.DataAnnotations;
using AstrolPOSAPI.Domain.Entities.POS;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreen.DTOs
{
    public class TouchScreenDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string ScreenName { get; set; } = default!;
        public string? Description { get; set; }
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public int DefaultFontSize { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string StoreOfOperationId { get; set; } = default!;
        public string? StoreOfOperationName { get; set; }
        public List<TouchScreenButtonDto>? Buttons { get; set; }
    }

    public class TouchScreenButtonDto
    {
        public string Id { get; set; } = default!;
        public string TouchScreenId { get; set; } = default!;
        public ButtonType ButtonType { get; set; }
        public string? ItemId { get; set; }
        public string ItemName { get; set; } = default!;
        public ButtonShape Shape { get; set; }
        public string BackgroundColor { get; set; } = default!;
        public string TextColor { get; set; } = default!;
        public int? FontSize { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public bool ShowImage { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDefaultImage { get; set; }
        public int SortOrder { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string StoreOfOperationId { get; set; } = default!;
        public string? StoreOfOperationName { get; set; }
    }

    public class CreateTouchScreenDto
    {
        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string ScreenName { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(1, 10)]
        public int GridRows { get; set; } = 2;

        [Range(1, 10)]
        public int GridColumns { get; set; } = 2;

        [Range(8, 72)]
        public int DefaultFontSize { get; set; } = 12;

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateTouchScreenDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string ScreenName { get; set; } = default!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(1, 10)]
        public int GridRows { get; set; }

        [Range(1, 10)]
        public int GridColumns { get; set; }

        [Range(8, 72)]
        public int DefaultFontSize { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class CreateTouchScreenButtonDto
    {
        [Required]
        public string TouchScreenId { get; set; } = default!;

        [Required]
        public ButtonType ButtonType { get; set; }

        public string? ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = default!;

        public ButtonShape Shape { get; set; } = ButtonShape.Rectangle;

        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
        public string BackgroundColor { get; set; } = "#FFFFFF";

        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
        public string TextColor { get; set; } = "#000000";

        [Range(8, 72)]
        public int? FontSize { get; set; }

        [Required]
        [Range(1, 100)]
        public int Row { get; set; }

        [Required]
        [Range(1, 100)]
        public int Column { get; set; }

        [Range(1, 10)]
        public int RowSpan { get; set; } = 1;

        [Range(1, 10)]
        public int ColumnSpan { get; set; } = 1;

        public bool ShowImage { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsDefaultImage { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateTouchScreenButtonDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        public string TouchScreenId { get; set; } = default!;

        [Required]
        public ButtonType ButtonType { get; set; }

        public string? ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = default!;

        public ButtonShape Shape { get; set; }

        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
        public string BackgroundColor { get; set; } = default!;

        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
        public string TextColor { get; set; } = default!;

        [Range(8, 72)]
        public int? FontSize { get; set; }

        [Required]
        [Range(1, 100)]
        public int Row { get; set; }

        [Required]
        [Range(1, 100)]
        public int Column { get; set; }

        [Range(1, 10)]
        public int RowSpan { get; set; }

        [Range(1, 10)]
        public int ColumnSpan { get; set; }

        public bool ShowImage { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsDefaultImage { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public string CompanyId { get; set; } = default!;

        [Required]
        public string StoreOfOperationId { get; set; } = default!;
    }
}

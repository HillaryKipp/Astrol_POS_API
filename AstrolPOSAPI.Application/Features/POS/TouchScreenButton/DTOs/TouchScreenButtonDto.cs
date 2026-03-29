using AstrolPOSAPI.Domain.Entities.POS;

namespace AstrolPOSAPI.Application.Features.POS.TouchScreenButton.DTOs
{
    public class TouchScreenButtonDto
    {
        public string Id { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int Row { get; set; }
        public int Column { get; set; }
        public int SortOrder { get; set; }
        public string BackgroundColor { get; set; } = string.Empty;
        public string TextColor { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public ButtonType ButtonType { get; set; }
        public ButtonShape Shape { get; set; }
        public string TouchScreenId { get; set; } = string.Empty;
    }
}

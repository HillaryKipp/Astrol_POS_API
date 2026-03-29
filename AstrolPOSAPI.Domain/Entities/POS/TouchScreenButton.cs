using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    public class TouchScreenButton : BaseAuditableEntity
    {
        public string ItemName { get; set; } = string.Empty;
        public int Row { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public int VerificationAttempts { get; set; }
        public int Column { get; set; }
        public int SortOrder { get; set; }
        public string BackgroundColor { get; set; } = "#FFFFFF";
        public string TextColor { get; set; } = "#000000";
        public string? ImageUrl { get; set; }

        public ButtonType ButtonType { get; set; }
        public ButtonShape Shape { get; set; }

        public string TouchScreenId { get; set; } = string.Empty;
        public TouchScreen? TouchScreen { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }

        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }
    }

    public enum ButtonType
    {
        Standard = 0,
        Item = 1,
        Category = 2
    }

    public enum ButtonShape
    {
        Rectangle = 0,
        Rounded = 1,
        Circle = 2
    }
}

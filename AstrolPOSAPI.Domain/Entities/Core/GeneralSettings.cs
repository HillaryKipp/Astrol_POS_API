using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Core
{
    public class GeneralSettings : BaseAuditableEntity
    {
        // Foreign key to Company
        public string CompanyId { get; set; } = default!;

        // Logo and branding
        public string? LogoUrl { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanySlogan { get; set; }

        // Theme colors
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public string? AccentColor { get; set; }
        public string? BackgroundColor { get; set; }

        // Background image
        public string? BackgroundImageUrl { get; set; }

        // Feature flags
        public bool HasOtp { get; set; }
        public bool EnableInventory { get; set; } = true;
        public bool EnablePOS { get; set; } = true;
        public bool EnableReporting { get; set; } = true;

        // Regional settings
        public string? Currency { get; set; } = "USD";
        public string? CurrencySymbol { get; set; } = "$";
        public string? DateFormat { get; set; } = "MM/dd/yyyy";
        public string? TimeFormat { get; set; } = "12"; // 12 or 24 hour
        public string? Timezone { get; set; }

        // Business settings
        public string? TaxNumber { get; set; }
        public decimal? DefaultTaxRate { get; set; }
        public string? ReceiptFooter { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }

        // Navigation property
        public Company? Company { get; set; }
    }
}

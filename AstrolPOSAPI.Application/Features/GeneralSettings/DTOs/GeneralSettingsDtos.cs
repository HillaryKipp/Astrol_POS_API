namespace AstrolPOSAPI.Application.Features.GeneralSettings.DTOs
{
    public class GeneralSettingsDto
    {
        public string Id { get; set; } = default!;
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
        public string? BackgroundImageUrl { get; set; }

        // Feature flags
        public bool HasOtp { get; set; }
        public bool EnableInventory { get; set; }
        public bool EnablePOS { get; set; }
        public bool EnableReporting { get; set; }

        // Regional settings
        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? Timezone { get; set; }

        // Business settings
        public string? TaxNumber { get; set; }
        public decimal? DefaultTaxRate { get; set; }
        public string? ReceiptFooter { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }
    }

    public class CreateGeneralSettingsDto
    {
        public string CompanyId { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public bool HasOtp { get; set; }
        public string? Currency { get; set; }
        public string? Timezone { get; set; }
    }

    public class UpdateGeneralSettingsDto
    {
        public string Id { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanySlogan { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? TertiaryColor { get; set; }
        public string? AccentColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public bool HasOtp { get; set; }
        public bool EnableInventory { get; set; }
        public bool EnablePOS { get; set; }
        public bool EnableReporting { get; set; }
        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? Timezone { get; set; }
        public string? TaxNumber { get; set; }
        public decimal? DefaultTaxRate { get; set; }
        public string? ReceiptFooter { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }
    }
}

using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class GeneralSettings : BaseAuditableEntity
    {
        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
        public string? LogoUrl { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanySlogan { get; set; }
        public string PrimaryColor { get; set; } = "#007bff";
        public string SecondaryColor { get; set; } = "#6c757d";
        public string TertiaryColor { get; set; } = "#28a745";
        public string? AccentColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public bool HasOtp { get; set; }
        public bool EnableInventory { get; set; } = true;
        public bool EnablePOS { get; set; } = true;
        public bool EnableReporting { get; set; } = true;
        public string Currency { get; set; } = "ksh";
        public string? CurrencySymbol { get; set; }
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string TimeFormat { get; set; } = "12";
        public string Timezone { get; set; } = "UTC";
        public string? TaxNumber { get; set; }
        public double? DefaultTaxRate { get; set; }
        public string? ReceiptFooter { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }
    }
}

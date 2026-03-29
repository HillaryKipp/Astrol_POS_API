using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    /// <summary>
    /// Configures VAT percentages that apply to items/services.
    /// Modeled after D365 BC VAT Posting Setup.
    /// </summary>
    public class VATPostingSetup : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// VAT percentage (e.g., 16 for 16%)
        /// </summary>
        public decimal VATPercentage { get; set; }

        /// <summary>
        /// GL Account code where VAT payable is posted
        /// </summary>
        public string? VATAccountCode { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}

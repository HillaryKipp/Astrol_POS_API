namespace AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs
{
    public class VATSetupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal VATPercentage { get; set; }
        public string? VATAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateVATSetupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal VATPercentage { get; set; }
        public string? VATAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateVATSetupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal VATPercentage { get; set; }
        public string? VATAccountCode { get; set; }
    }
}

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs
{
    public class VendorPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? PayablesAccountCode { get; set; }
        public string? ServiceChargeAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateVendorPostingGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string? PayablesAccountCode { get; set; }
        public string? ServiceChargeAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GenBusPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateGenBusPostingGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateVendorPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? PayablesAccountCode { get; set; }
        public string? ServiceChargeAccountCode { get; set; }
    }

    public class UpdateGenBusPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Name { get; set; }
    }
}

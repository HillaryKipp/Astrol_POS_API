using AstrolPOSAPI.Domain.Entities.Accounting;

namespace AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs
{
    public class VendorDto
    {
        public string Id { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? VATRegistrationNo { get; set; }
        public bool Blocked { get; set; }

        public string? VendorPostingGroupId { get; set; }
        public string? VendorPostingGroupCode { get; set; }

        public string? GenBusPostingGroupId { get; set; }
        public string? GenBusPostingGroupCode { get; set; }

        public string? ImageUrl { get; set; }
        public string? DocumentUrl { get; set; }

        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateVendorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? VATRegistrationNo { get; set; }

        public string? VendorPostingGroupId { get; set; }
        public string? GenBusPostingGroupId { get; set; }

        public string? ImageUrl { get; set; }
        public string? DocumentUrl { get; set; }

        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateVendorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? VATRegistrationNo { get; set; }
        public bool Blocked { get; set; }

        public string? VendorPostingGroupId { get; set; }
        public string? GenBusPostingGroupId { get; set; }

        public string? ImageUrl { get; set; }
        public string? DocumentUrl { get; set; }
    }
}

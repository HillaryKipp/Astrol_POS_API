using AstrolPOSAPI.Domain.Entities.Accounting;

namespace AstrolPOSAPI.Application.Features.Sales.Customer.DTOs
{
    public class CustomerDto
    {
        public string Id { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? CustomerPostingGroupId { get; set; }
        public string? CustomerPostingGroupCode { get; set; }

        public string? GenBusPostingGroupId { get; set; }
        public string? GenBusPostingGroupCode { get; set; }

        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? CustomerPostingGroupId { get; set; }
        public string? GenBusPostingGroupId { get; set; }

        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateCustomerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string? CustomerPostingGroupId { get; set; }
        public string? GenBusPostingGroupId { get; set; }
    }
}

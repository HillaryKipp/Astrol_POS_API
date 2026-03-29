namespace AstrolPOSAPI.Application.Features.Company.DTOs
{
    public class CreateCompanyDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}

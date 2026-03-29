namespace AstrolPOSAPI.Application.Features.Company.DTOs
{
    public class UpdateCompanyDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}

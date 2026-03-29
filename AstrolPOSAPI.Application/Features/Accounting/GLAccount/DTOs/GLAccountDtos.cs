using AstrolPOSAPI.Domain.Entities.Accounting;

namespace AstrolPOSAPI.Application.Features.Accounting.GLAccount.DTOs
{
    public class GLAccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public GLAccountType AccountType { get; set; }
        public bool DirectPosting { get; set; }
        public bool Blocked { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateGLAccountDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public GLAccountType AccountType { get; set; }
        public bool DirectPosting { get; set; } = true;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateGLAccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public GLAccountType AccountType { get; set; }
        public bool DirectPosting { get; set; }
        public bool Blocked { get; set; }
    }
}

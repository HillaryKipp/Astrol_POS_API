using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class GLAccount : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public GLAccountType AccountType { get; set; }
        public bool DirectPosting { get; set; } = true;
        public bool Blocked { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }

    public enum GLAccountType
    {
        Posting = 0,
        Heading = 1,
        Total = 2,
        BeginTotal = 3,
        EndTotal = 4
    }
}

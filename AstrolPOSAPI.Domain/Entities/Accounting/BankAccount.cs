using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.Accounting
{
    public class BankAccount : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public BankAccountType AccountType { get; set; }

        public string? GLAccountCode { get; set; }
        public decimal Balance { get; set; }

        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }

    public enum BankAccountType
    {
        Cash = 0,
        Bank = 1,
        MobileMoney = 2
    }
}

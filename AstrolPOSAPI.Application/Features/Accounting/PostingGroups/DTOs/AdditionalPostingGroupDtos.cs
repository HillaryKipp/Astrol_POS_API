using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Domain.Entities.Accounting;

namespace AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs
{
    // Customer Posting Group
    public class CustomerPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ReceivablesAccountCode { get; set; }
        public string? SalesAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateCustomerPostingGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ReceivablesAccountCode { get; set; }
        public string? SalesAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateCustomerPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ReceivablesAccountCode { get; set; }
        public string? SalesAccountCode { get; set; }
    }

    // Gen Prod Posting Group
    public class GenProdPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateGenProdPostingGroupDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateGenProdPostingGroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // General Posting Setup
    public class GeneralPostingSetupDto
    {
        public string Id { get; set; } = string.Empty;
        public string GenBusPostingGroupCode { get; set; } = string.Empty;
        public string GenProdPostingGroupCode { get; set; } = string.Empty;
        public string? SalesAccountCode { get; set; }
        public string? PurchaseAccountCode { get; set; }
        public string? COGSAccountCode { get; set; }
        public string? InventoryAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateGeneralPostingSetupDto
    {
        public string GenBusPostingGroupCode { get; set; } = string.Empty;
        public string GenProdPostingGroupCode { get; set; } = string.Empty;
        public string? SalesAccountCode { get; set; }
        public string? PurchaseAccountCode { get; set; }
        public string? COGSAccountCode { get; set; }
        public string? InventoryAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateGeneralPostingSetupDto
    {
        public string Id { get; set; } = string.Empty;
        public string? SalesAccountCode { get; set; }
        public string? PurchaseAccountCode { get; set; }
        public string? COGSAccountCode { get; set; }
        public string? InventoryAccountCode { get; set; }
    }

    // Bank Account
    public class BankAccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public BankAccountType AccountType { get; set; }
        public string? GLAccountCode { get; set; }
        public decimal Balance { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class CreateBankAccountDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public BankAccountType AccountType { get; set; }
        public string? GLAccountCode { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }

    public class UpdateBankAccountDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public BankAccountType AccountType { get; set; }
        public string? GLAccountCode { get; set; }
    }
}

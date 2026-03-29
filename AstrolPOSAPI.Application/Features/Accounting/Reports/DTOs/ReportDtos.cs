using System;
using System.Collections.Generic;

namespace AstrolPOSAPI.Application.Features.Accounting.Reports.DTOs
{
    public class TrialBalanceDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal OpeningBalance { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ClosingBalance { get; set; }
    }

    public class ProfitLossDto
    {
        public string Category { get; set; } = string.Empty; // Revenue, COGS, Expense
        public List<PLAccountLineDto> Lines { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    public class PLAccountLineDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class BalanceSheetDto
    {
        public List<BalanceSheetSectionDto> Assets { get; set; } = new();
        public List<BalanceSheetSectionDto> Liabilities { get; set; } = new();
        public List<BalanceSheetSectionDto> Equity { get; set; } = new();
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalEquity { get; set; }
    }

    public class BalanceSheetSectionDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }

    public class AgingReportDto
    {
        public string PartyNo { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public decimal Current { get; set; }
        public decimal Days1To30 { get; set; }
        public decimal Days31To60 { get; set; }
        public decimal Days61To90 { get; set; }
        public decimal Over90 { get; set; }
        public decimal Total { get; set; }
    }

    public class DailySalesSummaryDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public List<PaymentMethodSummaryDto> Payments { get; set; } = new();
    }

    public class PaymentMethodSummaryDto
    {
        public string Method { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }

    public class BankSummaryDto
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}

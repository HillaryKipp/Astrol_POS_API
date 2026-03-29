using System.Linq;
using AstrolPOSAPI.Application.Features.Accounting.Reports.DTOs;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Domain.Entities.Accounting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Application.Features.Accounting.Reports.Queries
{
    public class GetTrialBalanceQuery : IRequest<List<TrialBalanceDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetTrialBalanceQueryHandler : IRequestHandler<GetTrialBalanceQuery, List<TrialBalanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTrialBalanceQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<TrialBalanceDto>> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount>().Entities
                .Where(a => a.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            var entries = await _unitOfWork.Repository<GLEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.PostingDate <= request.EndDate)
                .ToListAsync(cancellationToken);

            var result = new List<TrialBalanceDto>();

            foreach (var account in accounts)
            {
                var accountEntries = entries.Where(e => e.GLAccountNo == account.Code).ToList();

                var opening = accountEntries.Where(e => e.PostingDate < request.StartDate).Sum(e => e.Amount);
                var periodDebit = accountEntries.Where(e => e.PostingDate >= request.StartDate && e.PostingDate <= request.EndDate).Sum(e => e.DebitAmount);
                var periodCredit = accountEntries.Where(e => e.PostingDate >= request.StartDate && e.PostingDate <= request.EndDate).Sum(e => e.CreditAmount);

                result.Add(new TrialBalanceDto
                {
                    AccountCode = account.Code,
                    AccountName = account.Name,
                    OpeningBalance = opening,
                    DebitAmount = periodDebit,
                    CreditAmount = periodCredit,
                    ClosingBalance = opening + periodDebit - periodCredit
                });
            }

            return result;
        }
    }

    public class GetProfitLossQuery : IRequest<List<ProfitLossDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetProfitLossQueryHandler : IRequestHandler<GetProfitLossQuery, List<ProfitLossDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetProfitLossQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<ProfitLossDto>> Handle(GetProfitLossQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount>().Entities
                .Where(a => a.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            var entries = await _unitOfWork.Repository<GLEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.PostingDate >= request.StartDate && e.PostingDate <= request.EndDate)
                .ToListAsync(cancellationToken);

            // BC Style: 4000-4999 Income, 5000-5999 COGS, 6000+ Expense (simplified mapping)
            var revenueAccounts = accounts.Where(a => a.Code.StartsWith("4")).ToList();
            var cogsAccounts = accounts.Where(a => a.Code.StartsWith("5")).ToList();
            var expenseAccounts = accounts.Where(a => a.Code.StartsWith("6") || a.Code.StartsWith("7") || a.Code.StartsWith("8")).ToList();

            var result = new List<ProfitLossDto>();

            // Revenue
            var revenueSection = new ProfitLossDto { Category = "Revenue" };
            foreach (var acc in revenueAccounts)
            {
                var amt = -entries.Where(e => e.GLAccountNo == acc.Code).Sum(e => e.Amount); // Revenue is usually credit (negative in BC)
                if (amt != 0) revenueSection.Lines.Add(new PLAccountLineDto { AccountCode = acc.Code, AccountName = acc.Name, Amount = amt });
            }
            revenueSection.TotalAmount = revenueSection.Lines.Sum(l => l.Amount);
            result.Add(revenueSection);

            // COGS
            var cogsSection = new ProfitLossDto { Category = "Cost of Goods Sold" };
            foreach (var acc in cogsAccounts)
            {
                var amt = entries.Where(e => e.GLAccountNo == acc.Code).Sum(e => e.Amount);
                if (amt != 0) cogsSection.Lines.Add(new PLAccountLineDto { AccountCode = acc.Code, AccountName = acc.Name, Amount = amt });
            }
            cogsSection.TotalAmount = cogsSection.Lines.Sum(l => l.Amount);
            result.Add(cogsSection);

            // Gross Profit
            result.Add(new ProfitLossDto { Category = "Gross Profit", TotalAmount = revenueSection.TotalAmount - cogsSection.TotalAmount });

            // Expenses
            var expenseSection = new ProfitLossDto { Category = "Expenses" };
            foreach (var acc in expenseAccounts)
            {
                var amt = entries.Where(e => e.GLAccountNo == acc.Code).Sum(e => e.Amount);
                if (amt != 0) expenseSection.Lines.Add(new PLAccountLineDto { AccountCode = acc.Code, AccountName = acc.Name, Amount = amt });
            }
            expenseSection.TotalAmount = expenseSection.Lines.Sum(l => l.Amount);
            result.Add(expenseSection);

            // Net Profit
            result.Add(new ProfitLossDto { Category = "Net Profit", TotalAmount = revenueSection.TotalAmount - cogsSection.TotalAmount - expenseSection.TotalAmount });

            return result;
        }
    }

    public class GetBalanceSheetQuery : IRequest<BalanceSheetDto>
    {
        public string CompanyId { get; set; } = string.Empty;
        public DateTime AsOfDate { get; set; }
    }

    public class GetBalanceSheetQueryHandler : IRequestHandler<GetBalanceSheetQuery, BalanceSheetDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBalanceSheetQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BalanceSheetDto> Handle(GetBalanceSheetQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount>().Entities
                .Where(a => a.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            var entries = await _unitOfWork.Repository<GLEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.PostingDate <= request.AsOfDate)
                .ToListAsync(cancellationToken);

            var report = new BalanceSheetDto();

            // Simplified BC mapping: 1000-1999 Assets, 2000-2999 Liabilities, 3000-3999 Equity
            foreach (var acc in accounts)
            {
                var balance = entries.Where(e => e.GLAccountNo == acc.Code).Sum(e => e.Amount);
                if (balance == 0) continue;

                var line = new BalanceSheetSectionDto { AccountCode = acc.Code, AccountName = acc.Name, Balance = Math.Abs(balance) };

                if (acc.Code.StartsWith("1")) report.Assets.Add(line);
                else if (acc.Code.StartsWith("2")) report.Liabilities.Add(line);
                else if (acc.Code.StartsWith("3")) report.Equity.Add(line);
            }

            // Include Retained Earnings (Net Profit from all time up to AsOfDate)
            // This is a simplified logic. In a real system, you'd close the income statement.
            var profitEntries = await _unitOfWork.Repository<GLEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.PostingDate <= request.AsOfDate && (e.GLAccountNo.StartsWith("4") || e.GLAccountNo.StartsWith("5") || e.GLAccountNo.StartsWith("6")))
                .SumAsync(e => e.Amount, cancellationToken);

            if (profitEntries != 0)
            {
                report.Equity.Add(new BalanceSheetSectionDto { AccountCode = "RE", AccountName = "Retained Earnings (Period Profit)", Balance = -profitEntries });
            }

            report.TotalAssets = report.Assets.Sum(a => a.Balance);
            report.TotalLiabilities = report.Liabilities.Sum(a => a.Balance);
            report.TotalEquity = report.Equity.Sum(a => a.Balance);

            return report;
        }
    }

    public class GetDailySalesSummaryQuery : IRequest<DailySalesSummaryDto>
    {
        public string CompanyId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class GetDailySalesSummaryQueryHandler : IRequestHandler<GetDailySalesSummaryQuery, DailySalesSummaryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDailySalesSummaryQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<DailySalesSummaryDto> Handle(GetDailySalesSummaryQuery request, CancellationToken cancellationToken)
        {
            var startOfDay = request.Date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            var orders = await _unitOfWork.Repository<AstrolPOSAPI.Domain.Entities.POS.SalesOrder>().Entities
                .Include(o => o.Payments)
                .Where(o => o.CompanyId == request.CompanyId && o.OrderDate >= startOfDay && o.OrderDate <= endOfDay && o.Status == AstrolPOSAPI.Domain.Entities.POS.SalesOrderStatus.Completed)
                .ToListAsync(cancellationToken);

            var summary = new DailySalesSummaryDto
            {
                Date = startOfDay,
                TotalSales = orders.Sum(o => o.TotalAmount),
                OrderCount = orders.Count
            };

            var payments = orders.SelectMany(o => o.Payments).Where(p => p.Status == AstrolPOSAPI.Domain.Entities.POS.PaymentStatus.Completed).ToList();

            foreach (var group in payments.GroupBy(p => p.PaymentMethod))
            {
                summary.Payments.Add(new PaymentMethodSummaryDto
                {
                    Method = group.Key.ToString(),
                    Amount = group.Sum(p => p.Amount),
                    Count = group.Count()
                });
            }

            return summary;
        }
    }

    public class GetCustomerAgingQuery : IRequest<List<AgingReportDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetCustomerAgingQueryHandler : IRequestHandler<GetCustomerAgingQuery, List<AgingReportDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCustomerAgingQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<AgingReportDto>> Handle(GetCustomerAgingQuery request, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.Repository<Customer>().Entities
                .Where(c => c.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            var entries = await _unitOfWork.Repository<CustomerLedgerEntry>().Entities
                .Where(e => e.CompanyId == request.CompanyId && e.Open)
                .ToListAsync(cancellationToken);

            var report = new List<AgingReportDto>();
            var today = DateTime.UtcNow.Date;

            foreach (var cust in customers)
            {
                var custEntries = entries.Where(e => e.CustomerNo == cust.No).ToList();
                if (!custEntries.Any()) continue;

                var aging = new AgingReportDto
                {
                    PartyNo = cust.No,
                    PartyName = cust.Name,
                    Total = custEntries.Sum(e => e.RemainingAmount)
                };

                foreach (var entry in custEntries)
                {
                    var days = (today - entry.PostingDate.Date).Days;
                    if (days <= 0) aging.Current += entry.RemainingAmount;
                    else if (days <= 30) aging.Days1To30 += entry.RemainingAmount;
                    else if (days <= 60) aging.Days31To60 += entry.RemainingAmount;
                    else if (days <= 90) aging.Days61To90 += entry.RemainingAmount;
                    else aging.Over90 += entry.RemainingAmount;
                }

                report.Add(aging);
            }

            return report;
        }
    }

    public class GetBankSummaryQuery : IRequest<List<BankSummaryDto>>
    {
        public string CompanyId { get; set; } = string.Empty;
    }

    public class GetBankSummaryQueryHandler : IRequestHandler<GetBankSummaryQuery, List<BankSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBankSummaryQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<BankSummaryDto>> Handle(GetBankSummaryQuery request, CancellationToken cancellationToken)
        {
            var banks = await _unitOfWork.Repository<BankAccount>().Entities
                .Where(b => b.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            return banks.Select(b => new BankSummaryDto
            {
                AccountCode = b.Code,
                AccountName = b.Name,
                Type = b.AccountType.ToString(),
                Balance = b.Balance
            }).ToList();
        }
    }
}

using AstrolPOSAPI.Application.Features.Accounting.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("trial-balance")]
        public async Task<IActionResult> GetTrialBalance([FromQuery] string companyId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(await _mediator.Send(new GetTrialBalanceQuery { CompanyId = companyId, StartDate = startDate, EndDate = endDate }));
        }

        [HttpGet("profit-loss")]
        public async Task<IActionResult> GetProfitLoss([FromQuery] string companyId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(await _mediator.Send(new GetProfitLossQuery { CompanyId = companyId, StartDate = startDate, EndDate = endDate }));
        }

        [HttpGet("balance-sheet")]
        public async Task<IActionResult> GetBalanceSheet([FromQuery] string companyId, [FromQuery] DateTime asOfDate)
        {
            return Ok(await _mediator.Send(new GetBalanceSheetQuery { CompanyId = companyId, AsOfDate = asOfDate }));
        }

        [HttpGet("daily-sales")]
        public async Task<IActionResult> GetDailySalesSummary([FromQuery] string companyId, [FromQuery] DateTime date)
        {
            return Ok(await _mediator.Send(new GetDailySalesSummaryQuery { CompanyId = companyId, Date = date }));
        }

        [HttpGet("customer-aging")]
        public async Task<IActionResult> GetCustomerAging([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetCustomerAgingQuery { CompanyId = companyId }));
        }

        [HttpGet("bank-summary")]
        public async Task<IActionResult> GetBankSummary([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetBankSummaryQuery { CompanyId = companyId }));
        }
    }
}

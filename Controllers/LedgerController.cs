using AstrolPOSAPI.Application.Features.Accounting.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LedgerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LedgerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("gl-entries")]
        public async Task<IActionResult> GetGLEntries([FromQuery] string companyId, [FromQuery] string? accountNo)
        {
            return Ok(await _mediator.Send(new GetGLEntriesQuery { CompanyId = companyId, GLAccountNo = accountNo }));
        }

        [HttpGet("vendor-ledger")]
        public async Task<IActionResult> GetVendorLedger([FromQuery] string companyId, [FromQuery] string? vendorNo, [FromQuery] bool? openOnly)
        {
            return Ok(await _mediator.Send(new GetVendorLedgerEntriesQuery { CompanyId = companyId, VendorNo = vendorNo, OpenOnly = openOnly }));
        }
    }
}

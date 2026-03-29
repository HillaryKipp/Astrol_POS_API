using AstrolPOSAPI.Application.Features.Purchasing.Invoice.Commands;
using AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs;
using AstrolPOSAPI.Application.Features.Purchasing.Invoice.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchasingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetAllInvoices([FromQuery] string companyId, [FromQuery] bool includePosted = false)
        {
            return Ok(await _mediator.Send(new GetAllPurchaseInvoicesQuery { CompanyId = companyId, IncludePosted = includePosted }));
        }

        [HttpGet("invoices/{id}")]
        public async Task<IActionResult> GetInvoice(string id, [FromQuery] bool isPosted = false)
        {
            if (isPosted)
            {
                return Ok(await _mediator.Send(new GetPostedPurchaseInvoiceQuery { Id = id }));
            }
            return Ok(await _mediator.Send(new GetPurchaseInvoiceQuery { Id = id, IsPosted = isPosted }));
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreatePurchaseHeaderDto invoice)
        {
            return Ok(await _mediator.Send(new CreatePurchaseInvoiceCommand { Invoice = invoice }));
        }

        [HttpPut("invoices/{id}")]
        public async Task<IActionResult> UpdateInvoice(string id, [FromBody] CreatePurchaseHeaderDto invoice)
        {
            return Ok(await _mediator.Send(new UpdatePurchaseInvoiceCommand { Id = id, Invoice = invoice }));
        }

        [HttpDelete("invoices/{id}")]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            return Ok(await _mediator.Send(new DeletePurchaseInvoiceCommand { Id = id }));
        }

        [HttpPost("invoices/{id}/post")]
        public async Task<IActionResult> PostInvoice(string id)
        {
            return Ok(await _mediator.Send(new PostPurchaseInvoiceCommand { PurchaseHeaderId = id }));
        }
    }
}

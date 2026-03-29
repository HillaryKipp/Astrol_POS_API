using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Commands;
using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.DTOs;
using AstrolPOSAPI.Application.Features.Purchasing.PaymentVoucher.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentVoucherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentVoucherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllPaymentVouchersQuery { CompanyId = companyId }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new GetPaymentVoucherQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentVoucherDto dto)
        {
            return Ok(await _mediator.Send(new CreatePaymentVoucherCommand { PaymentVoucher = dto }));
        }

        [HttpPost("{id}/post")]
        public async Task<IActionResult> Post(string id)
        {
            return Ok(await _mediator.Send(new PostPaymentVoucherCommand { PaymentVoucherId = id }));
        }
    }
}

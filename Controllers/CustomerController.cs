using AstrolPOSAPI.Application.Features.Sales.Customer.Commands;
using AstrolPOSAPI.Application.Features.Sales.Customer.DTOs;
using AstrolPOSAPI.Application.Features.Sales.Customer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllCustomersQuery { CompanyId = companyId }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new GetCustomerByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto customer)
        {
            return Ok(await _mediator.Send(new CreateCustomerCommand { Customer = customer }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto customer)
        {
            return Ok(await _mediator.Send(new UpdateCustomerCommand { Customer = customer }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new DeleteCustomerCommand { Id = id }));
        }
    }
}

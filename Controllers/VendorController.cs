using AstrolPOSAPI.Application.Features.Purchasing.Vendor.Commands;
using AstrolPOSAPI.Application.Features.Purchasing.Vendor.DTOs;
using AstrolPOSAPI.Application.Features.Purchasing.Vendor.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllVendorsQuery { CompanyId = companyId }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new GetVendorByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorDto vendor)
        {
            return Ok(await _mediator.Send(new CreateVendorCommand { Vendor = vendor }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVendorDto vendor)
        {
            return Ok(await _mediator.Send(new UpdateVendorCommand { Vendor = vendor }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new DeleteVendorCommand { Id = id }));
        }
    }
}

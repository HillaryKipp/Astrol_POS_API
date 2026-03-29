using AstrolPOSAPI.Application.Features.Store.Commands.CreateStore;
using AstrolPOSAPI.Application.Features.Store.DTOs;
using AstrolPOSAPI.Application.Features.Store.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<StoreDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? companyId)
        {
            var stores = await _mediator.Send(new GetAllStoresQuery { CompanyId = companyId });
            return Ok(stores);
        }

        [HttpPost]
        [ProducesResponseType(typeof(StoreDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateStoreCommand command)
        {
            try
            {
                var store = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAll), new { id = store.Id }, store);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

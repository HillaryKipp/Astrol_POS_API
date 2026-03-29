using AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.CreateTouchScreen;
using AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.DeleteTouchScreen;
using AstrolPOSAPI.Application.Features.POS.TouchScreen.Commands.UpdateTouchScreen;
using AstrolPOSAPI.Application.Features.POS.TouchScreen.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TouchScreenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TouchScreenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId,
            [FromQuery] bool includeButtons = false)
        {
            var query = new GetAllTouchScreensQuery
            {
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId,
                IncludeButtons = includeButtons
            };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] bool includeButtons = true)
        {
            var query = new GetTouchScreenByIdQuery
            {
                Id = id,
                IncludeButtons = includeButtons
            };
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTouchScreenCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTouchScreenCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteTouchScreenCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

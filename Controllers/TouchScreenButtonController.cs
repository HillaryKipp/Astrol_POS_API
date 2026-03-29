using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.CreateTouchScreenButton;
using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.DeleteTouchScreenButton;
using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Commands.UpdateTouchScreenButton;
using AstrolPOSAPI.Application.Features.POS.TouchScreenButton.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TouchScreenButtonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TouchScreenButtonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? touchScreenId,
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId)
        {
            var query = new GetAllTouchScreenButtonsQuery
            {
                TouchScreenId = touchScreenId,
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId
            };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetTouchScreenButtonByIdQuery { Id = id };
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
        public async Task<IActionResult> Create([FromBody] CreateTouchScreenButtonCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTouchScreenButtonCommand command)
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
            var command = new DeleteTouchScreenButtonCommand { Id = id };
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

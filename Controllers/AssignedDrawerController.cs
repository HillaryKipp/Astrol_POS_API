using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.CreateAssignedDrawer;
using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.DeleteAssignedDrawer;
using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Commands.UpdateAssignedDrawer;
using AstrolPOSAPI.Application.Features.POS.AssignedDrawer.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignedDrawerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssignedDrawerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId,
            [FromQuery] string? userId,
            [FromQuery] string? drawerId)
        {
            var query = new GetAllAssignedDrawersQuery
            {
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId,
                UserId = userId,
                DrawerId = drawerId
            };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetAssignedDrawerByIdQuery { Id = id };
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
        public async Task<IActionResult> Create([FromBody] CreateAssignedDrawerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateAssignedDrawerCommand command)
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
            var command = new DeleteAssignedDrawerCommand { Id = id };
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

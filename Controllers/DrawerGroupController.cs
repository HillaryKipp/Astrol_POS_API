 using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.CreateDrawerGroup;
using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.DeleteDrawerGroup;
using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Commands.UpdateDrawerGroup;
using AstrolPOSAPI.Application.Features.POS.DrawerGroup.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DrawerGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DrawerGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? companyId, [FromQuery] string? storeOfOperationId)
        {
            var query = new GetAllDrawerGroupsQuery
            {
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId
            };
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetDrawerGroupByIdQuery { Id = id };
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
        public async Task<IActionResult> Create([FromBody] CreateDrawerGroupCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDrawerGroupCommand command)
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
            var command = new DeleteDrawerGroupCommand { Id = id };
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

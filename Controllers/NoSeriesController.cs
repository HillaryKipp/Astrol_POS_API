//using AstrolPOSAPI.Application.Features.NoSeries.Commands.CreateNoSeries;
using AstrolPOSAPI.Application.Features.NoSeries.Commands.CreateNoSeries;
using AstrolPOSAPI.Application.Features.NoSeries.Commands.DeleteNoSeries;
using AstrolPOSAPI.Application.Features.NoSeries.Commands.UpdateNoSeries;
using AstrolPOSAPI.Application.Features.NoSeries.DTOs;
using AstrolPOSAPI.Application.Features.NoSeries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoSeriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoSeriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<NoSeriesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var noSeries = await _mediator.Send(new GetAllNoSeriesQuery());
            return Ok(noSeries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoSeriesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var noSeries = await _mediator.Send(new GetNoSeriesByIdQuery { Id = id });
                return Ok(noSeries);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"NoSeries with ID {id} not found" });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(NoSeriesDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNoSeriesCommand command)
        {
            var noSeries = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = noSeries.Id }, noSeries);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NoSeriesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateNoSeriesCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                var noSeries = await _mediator.Send(command);
                return Ok(noSeries);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"NoSeries with ID {id} not found" });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeleteNoSeriesCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"NoSeries with ID {id} not found" });
            }
        }
    }
}

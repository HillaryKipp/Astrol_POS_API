using AstrolPOSAPI.Application.Features.GeneralSettings.Commands.CreateGeneralSettings;
using AstrolPOSAPI.Application.Features.GeneralSettings.Commands.UpdateGeneralSettings;
using AstrolPOSAPI.Application.Features.GeneralSettings.DTOs;
using AstrolPOSAPI.Application.Features.GeneralSettings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     [Authorize]
    public class GeneralSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GeneralSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("company/{companyId}")]
        [ProducesResponseType(typeof(GeneralSettingsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCompanyId(string companyId)
        {
            try
            {
                var settings = await _mediator.Send(new GetGeneralSettingsByCompanyIdQuery { CompanyId = companyId });
                return Ok(settings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"GeneralSettings for Company {companyId} not found" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GeneralSettingsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateGeneralSettingsCommand command)
        {
            try
            {
                var settings = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetByCompanyId), new { companyId = settings.CompanyId }, settings);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GeneralSettingsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateGeneralSettingsCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                var settings = await _mediator.Send(command);
                return Ok(settings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"GeneralSettings with ID {id} not found" });
            }
        }
    }
}

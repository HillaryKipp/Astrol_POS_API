using AstrolPOSAPI.Application.Features.Permission.Commands;
using AstrolPOSAPI.Application.Features.Permission.DTOs;
using AstrolPOSAPI.Application.Features.Permission.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only admins can manage permissions
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get permissions for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var permissions = await _mediator.Send(new GetPermissionsByUserIdQuery { UserId = userId });
            return Ok(permissions);
        }

        /// <summary>
        /// Create/Assign permission to user or role
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
        {
            try
            {
                var permission = await _mediator.Send(new CreatePermissionCommand
                {
                    UserId = dto.UserId,
                    RoleId = dto.RoleId,
                    ResourceName = dto.ResourceName,
                    CanWrite = dto.CanWrite,
                    CanRead = dto.CanRead,
                    CanEdit = dto.CanEdit,
                    CanDelete = dto.CanDelete
                });

                return CreatedAtAction(nameof(GetByUserId), new { userId = permission.UserId }, permission);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete permission (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeletePermissionCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Permission with ID {id} not found" });
            }
        }
    }
}

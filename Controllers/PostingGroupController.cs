using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Commands;
using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostingGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostingGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Vendor Posting Groups

        [HttpGet("vendor")]
        public async Task<IActionResult> GetAllVendorPostingGroups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllVendorPostingGroupsQuery { CompanyId = companyId }));
        }

        [HttpPost("vendor")]
        public async Task<IActionResult> CreateVendorPostingGroup([FromBody] CreateVendorPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new CreateVendorPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpPut("vendor")]
        public async Task<IActionResult> UpdateVendorPostingGroup([FromBody] UpdateVendorPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateVendorPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpDelete("vendor/{id}")]
        public async Task<IActionResult> DeleteVendorPostingGroup(string id)
        {
            return Ok(await _mediator.Send(new DeleteVendorPostingGroupCommand { Id = id }));
        }

        #endregion

        #region Gen Bus Posting Groups

        [HttpGet("gen-bus")]
        public async Task<IActionResult> GetAllGenBusPostingGroups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllGenBusPostingGroupsQuery { CompanyId = companyId }));
        }

        [HttpPost("gen-bus")]
        public async Task<IActionResult> CreateGenBusPostingGroup([FromBody] CreateGenBusPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new CreateGenBusPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpPut("gen-bus")]
        public async Task<IActionResult> UpdateGenBusPostingGroup([FromBody] UpdateGenBusPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateGenBusPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpDelete("gen-bus/{id}")]
        public async Task<IActionResult> DeleteGenBusPostingGroup(string id)
        {
            return Ok(await _mediator.Send(new DeleteGenBusPostingGroupCommand { Id = id }));
        }

        #endregion
    }
}

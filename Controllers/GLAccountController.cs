using AstrolPOSAPI.Application.Features.Accounting.GLAccount.Commands;
using AstrolPOSAPI.Application.Features.Accounting.GLAccount.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GLAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GLAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllGLAccountsQuery { CompanyId = companyId }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new GetGLAccountByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGLAccountDto dto)
        {
            return Ok(await _mediator.Send(new CreateGLAccountCommand { Account = dto }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateGLAccountDto dto)
        {
            return Ok(await _mediator.Send(new UpdateGLAccountCommand { Account = dto }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new DeleteGLAccountCommand { Id = id }));
        }
    }
}

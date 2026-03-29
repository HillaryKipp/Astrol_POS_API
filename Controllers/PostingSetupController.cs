using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Commands;
using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.DTOs;
using AstrolPOSAPI.Application.Features.Accounting.PostingGroups.Queries;
using AstrolPOSAPI.Application.Features.Accounting.VATSetup.Commands;
using AstrolPOSAPI.Application.Features.Accounting.VATSetup.DTOs;
using AstrolPOSAPI.Application.Features.Accounting.VATSetup.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostingSetupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostingSetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Customer Posting Group

        [HttpGet("customer")]
        public async Task<IActionResult> GetAllCustomerPostingGroups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllCustomerPostingGroupsQuery { CompanyId = companyId }));
        }

        [HttpPost("customer")]
        public async Task<IActionResult> CreateCustomerPostingGroup([FromBody] CreateCustomerPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new CreateCustomerPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpPut("customer")]
        public async Task<IActionResult> UpdateCustomerPostingGroup([FromBody] UpdateCustomerPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateCustomerPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpDelete("customer/{id}")]
        public async Task<IActionResult> DeleteCustomerPostingGroup(string id)
        {
            return Ok(await _mediator.Send(new DeleteCustomerPostingGroupCommand { Id = id }));
        }

        #endregion

        #region Gen Bus Posting Group

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

        #region Gen Prod Posting Group

        [HttpGet("gen-prod")]
        public async Task<IActionResult> GetAllGenProdPostingGroups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllGenProdPostingGroupsQuery { CompanyId = companyId }));
        }

        [HttpPost("gen-prod")]
        public async Task<IActionResult> CreateGenProdPostingGroup([FromBody] CreateGenProdPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new CreateGenProdPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpPut("gen-prod")]
        public async Task<IActionResult> UpdateGenProdPostingGroup([FromBody] UpdateGenProdPostingGroupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateGenProdPostingGroupCommand { PostingGroup = dto }));
        }

        [HttpDelete("gen-prod/{id}")]
        public async Task<IActionResult> DeleteGenProdPostingGroup(string id)
        {
            return Ok(await _mediator.Send(new DeleteGenProdPostingGroupCommand { Id = id }));
        }

        #endregion

        #region General Posting Setup

        [HttpGet("general-setup")]
        public async Task<IActionResult> GetAllGeneralPostingSetups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllGeneralPostingSetupsQuery { CompanyId = companyId }));
        }

        [HttpPost("general-setup")]
        public async Task<IActionResult> CreateGeneralPostingSetup([FromBody] CreateGeneralPostingSetupDto dto)
        {
            return Ok(await _mediator.Send(new CreateGeneralPostingSetupCommand { PostingSetup = dto }));
        }

        [HttpPut("general-setup")]
        public async Task<IActionResult> UpdateGeneralPostingSetup([FromBody] UpdateGeneralPostingSetupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateGeneralPostingSetupCommand { PostingSetup = dto }));
        }

        [HttpDelete("general-setup/{id}")]
        public async Task<IActionResult> DeleteGeneralPostingSetup(string id)
        {
            return Ok(await _mediator.Send(new DeleteGeneralPostingSetupCommand { Id = id }));
        }

        #endregion

        #region Bank Account

        [HttpGet("bank")]
        public async Task<IActionResult> GetAllBankAccounts([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllBankAccountsQuery { CompanyId = companyId }));
        }

        [HttpPost("bank")]
        public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountDto dto)
        {
            return Ok(await _mediator.Send(new CreateBankAccountCommand { BankAccount = dto }));
        }

        [HttpPut("bank")]
        public async Task<IActionResult> UpdateBankAccount([FromBody] UpdateBankAccountDto dto)
        {
            return Ok(await _mediator.Send(new UpdateBankAccountCommand { BankAccount = dto }));
        }

        [HttpDelete("bank/{id}")]
        public async Task<IActionResult> DeleteBankAccount(string id)
        {
            return Ok(await _mediator.Send(new DeleteBankAccountCommand { Id = id }));
        }

        #endregion

        #region VAT Posting Setup

        [HttpGet("vat")]
        public async Task<IActionResult> GetAllVATSetups([FromQuery] string companyId)
        {
            return Ok(await _mediator.Send(new GetAllVATSetupsQuery { CompanyId = companyId }));
        }

        [HttpGet("vat/{id}")]
        public async Task<IActionResult> GetVATSetup(string id)
        {
            return Ok(await _mediator.Send(new GetVATSetupQuery { Id = id }));
        }

        [HttpPost("vat")]
        public async Task<IActionResult> CreateVATSetup([FromBody] CreateVATSetupDto dto)
        {
            return Ok(await _mediator.Send(new CreateVATSetupCommand { VATSetup = dto }));
        }

        [HttpPut("vat/{id}")]
        public async Task<IActionResult> UpdateVATSetup(string id, [FromBody] UpdateVATSetupDto dto)
        {
            return Ok(await _mediator.Send(new UpdateVATSetupCommand { Id = id, VATSetup = dto }));
        }

        [HttpDelete("vat/{id}")]
        public async Task<IActionResult> DeleteVATSetup(string id)
        {
            return Ok(await _mediator.Send(new DeleteVATSetupCommand { Id = id }));
        }

        #endregion
    }
}

using AstrolPOSAPI.Application.Features.Company.Commands.CreateCompany;
using AstrolPOSAPI.Application.Features.Company.Commands.DeleteCompany;
using AstrolPOSAPI.Application.Features.Company.Commands.UpdateCompany;
using AstrolPOSAPI.Application.Features.Company.DTOs;
using AstrolPOSAPI.Application.Features.Company.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        /// Get all companies

        [HttpGet]
        [ProducesResponseType(typeof(List<CompanyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _mediator.Send(new GetAllCompaniesQuery());
            return Ok(companies);
        }


        /// Get company by ID

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {   
            try
            {
                var company = await _mediator.Send(new GetCompanyByIdQuery { Id = id });
                return Ok(company);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Company with ID {id} not found" });
            }
        }


        /// Create a new company

        [HttpPost]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
        {
            var company = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }


        /// Update an existing company

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCompanyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { message = "ID in URL does not match ID in request body" });
            }

            try
            {
                var company = await _mediator.Send(command);
                return Ok(company);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Company with ID {id} not found" });
            }
        }


        /// Delete a company (soft delete)

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeleteCompanyCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Company with ID {id} not found" });
            }
        }
    }
}

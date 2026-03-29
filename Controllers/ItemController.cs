using AstrolPOSAPI.Application.Features.POS.Item.Commands.CreateItem;
using AstrolPOSAPI.Application.Features.POS.Item.Commands.DeleteItem;
using AstrolPOSAPI.Application.Features.POS.Item.Commands.UpdateItem;
using AstrolPOSAPI.Application.Features.POS.Item.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all items with optional filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId,
            [FromQuery] string? categoryId,
            [FromQuery] bool? isActive,
            [FromQuery] string? searchTerm)
        {
            var query = new GetAllItemsQuery
            {
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId,
                CategoryId = categoryId,
                IsActive = isActive,
                SearchTerm = searchTerm
            };
            return Ok(await _mediator.Send(query));
        }

        /// <summary>
        /// Get item by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetItemByIdQuery { Id = id };
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

        /// <summary>
        /// Look up an item by its barcode (for barcode scanner)
        /// </summary>
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(
            string barcode,
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId)
        {
            var query = new GetItemByBarcodeQuery
            {
                Barcode = barcode,
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId
            };
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

        /// <summary>
        /// Create a new item
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateItemCommand command)
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

        /// <summary>
        /// Delete an item (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteItemCommand { Id = id };
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

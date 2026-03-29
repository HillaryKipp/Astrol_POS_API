using AstrolPOSAPI.Application.Features.POS.Sales.Commands.AddItemToSale;
using AstrolPOSAPI.Application.Features.POS.Sales.Commands.CompleteSale;
using AstrolPOSAPI.Application.Features.POS.Sales.Commands.CreateSalesOrder;
using AstrolPOSAPI.Application.Features.POS.Sales.Commands.ProcessPayment;
using AstrolPOSAPI.Application.Features.POS.Sales.Commands.RemoveItemFromSale;
using AstrolPOSAPI.Application.Features.POS.Sales.Commands.VoidSale;
using AstrolPOSAPI.Application.Features.POS.Sales.DTOs;
using AstrolPOSAPI.Application.Features.POS.Sales.Queries;
using AstrolPOSAPI.Domain.Entities.POS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new sales order (start a new sale)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSalesOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get sales order by ID with lines and payments
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetSalesOrderByIdQuery { Id = id };
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
        /// Get open sale for a drawer/cashier
        /// </summary>
        [HttpGet("open")]
        public async Task<IActionResult> GetOpenSale([FromQuery] string drawerId, [FromQuery] string? cashierId)
        {
            var query = new GetOpenSalesByDrawerQuery
            {
                DrawerId = drawerId,
                CashierId = cashierId
            };
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound(new { message = "No open sale found for this drawer" });
            return Ok(result);
        }

        /// <summary>
        /// Get sales history with filters
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] string? companyId,
            [FromQuery] string? storeOfOperationId,
            [FromQuery] string? drawerId,
            [FromQuery] string? cashierId,
            [FromQuery] SalesOrderStatus? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetSalesHistoryQuery
            {
                CompanyId = companyId,
                StoreOfOperationId = storeOfOperationId,
                DrawerId = drawerId,
                CashierId = cashierId,
                Status = status,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return Ok(await _mediator.Send(query));
        }

        /// <summary>
        /// Add item to sale
        /// </summary>
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(string id, [FromBody] AddItemToSaleDto dto)
        {
            try
            {
                var command = new AddItemToSaleCommand
                {
                    SalesOrderId = id,
                    ItemId = dto.ItemId,
                    Quantity = dto.Quantity,
                    DiscountAmount = dto.DiscountAmount ?? 0
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove item from sale
        /// </summary>
        [HttpDelete("{id}/items/{lineId}")]
        public async Task<IActionResult> RemoveItem(string id, string lineId)
        {
            try
            {
                var command = new RemoveItemFromSaleCommand
                {
                    SalesOrderId = id,
                    LineId = lineId
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process payment for a sale (Cash, M-Pesa, Visa, etc.)
        /// </summary>
        [HttpPost("{id}/pay")]
        public async Task<IActionResult> ProcessPayment(string id, [FromBody] ProcessPaymentDto dto)
        {
            try
            {
                var command = new ProcessPaymentCommand
                {
                    SalesOrderId = id,
                    PaymentMethod = dto.PaymentMethod,
                    Amount = dto.Amount,
                    PhoneNumber = dto.PhoneNumber,
                    ReferenceNo = dto.ReferenceNo,
                    CardLastFour = dto.CardLastFour
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Complete sale - finalizes order, deducts inventory, generates receipt
        /// </summary>
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteSale(string id, [FromBody] CompleteSaleRequest? request = null)
        {
            try
            {
                var command = new CompleteSaleCommand
                {
                    SalesOrderId = id,
                    PrintReceipt = request?.PrintReceipt ?? true,
                    EmailAddress = request?.EmailAddress,
                    PhoneNumber = request?.PhoneNumber
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Void a pending sale
        /// </summary>
        [HttpPost("{id}/void")]
        public async Task<IActionResult> VoidSale(string id, [FromBody] VoidSaleRequest? request = null)
        {
            try
            {
                var command = new VoidSaleCommand
                {
                    SalesOrderId = id,
                    Reason = request?.Reason
                };
                await _mediator.Send(command);
                return Ok(new { message = "Sale voided successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CompleteSaleRequest
    {
        public bool PrintReceipt { get; set; } = true;
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class VoidSaleRequest
    {
        public string? Reason { get; set; }
    }
}

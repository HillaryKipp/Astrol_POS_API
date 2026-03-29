using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Application.Models.Mpesa;
using AstrolPOSAPI.Domain.Entities.POS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MpesaController : ControllerBase
    {
        private readonly IMpesaService _mpesaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MpesaController> _logger;

        public MpesaController(
            IMpesaService mpesaService,
            IUnitOfWork unitOfWork,
            ILogger<MpesaController> logger)
        {
            _mpesaService = mpesaService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// M-Pesa STK Push callback endpoint (called by Safaricom)
        /// </summary>
        [HttpPost("callback")]
        [AllowAnonymous] // Must be accessible by Safaricom
        public async Task<IActionResult> StkCallback([FromBody] StkPushCallback callback)
        {
            _logger.LogInformation("Received M-Pesa STK callback");

            try
            {
                var result = await _mpesaService.ProcessCallbackAsync(callback);

                if (!result.Success)
                {
                    _logger.LogWarning(
                        "M-Pesa payment failed. CheckoutRequestId: {CheckoutId}, Message: {Message}",
                        result.CheckoutRequestId, result.Message);

                    // Still update the payment record as failed
                    await UpdatePaymentStatus(result, PaymentStatus.Failed);

                    // M-Pesa expects 200 OK regardless of result
                    return Ok(new { ResultCode = 0, ResultDesc = "Callback received" });
                }

                // Update payment record with success
                await UpdatePaymentStatus(result, PaymentStatus.Completed);

                _logger.LogInformation(
                    "M-Pesa payment confirmed. Receipt: {Receipt}, Amount: {Amount}",
                    result.MpesaReceiptNumber, result.Amount);

                return Ok(new { ResultCode = 0, ResultDesc = "Callback received" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing M-Pesa callback");
                // Still return 200 to prevent Safaricom from retrying
                return Ok(new { ResultCode = 0, ResultDesc = "Callback received" });
            }
        }

        /// <summary>
        /// Query the status of an STK Push transaction
        /// </summary>
        [HttpGet("status/{checkoutRequestId}")]
        [Authorize]
        public async Task<IActionResult> QueryStatus(string checkoutRequestId)
        {
            var result = await _mpesaService.QueryStkStatusAsync(checkoutRequestId);
            return Ok(result);
        }

        /// <summary>
        /// Test endpoint to initiate STK Push (for testing only)
        /// </summary>
        [HttpPost("test-stk")]
        [Authorize]
        public async Task<IActionResult> TestStkPush([FromBody] TestStkRequest request)
        {
            var result = await _mpesaService.InitiateStkPushAsync(
                request.PhoneNumber,
                request.Amount,
                request.Reference ?? "TEST",
                "Test Payment");

            return Ok(result);
        }

        /// <summary>
        /// Register C2B URLs (Admin only)
        /// </summary>
        [HttpPost("c2b/register-url")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterC2BUrl([FromBody] C2BRegisterUrlRequest request)
        {
            var result = await _mpesaService.RegisterC2BUrlsAsync(
                request.ShortCode,
                request.ConfirmationURL,
                request.ValidationURL);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// M-Pesa C2B Validation Callback
        /// </summary>
        [HttpPost("c2b/validation")]
        [AllowAnonymous]
        public async Task<IActionResult> C2BValidation([FromBody] C2BValidationRequest request)
        {
            var response = await _mpesaService.ValidateC2BPaymentAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// M-Pesa C2B Confirmation Callback
        /// </summary>
        [HttpPost("c2b/confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> C2BConfirmation([FromBody] C2BConfirmationRequest request)
        {
            var result = await _mpesaService.ProcessC2BConfirmationAsync(request);

            // Update payment stats if successful match found
            if (result.Success)
            {
                await UpdatePaymentStatus(result, PaymentStatus.Completed);
            }

            return Ok(new { ResultCode = "0", ResultDesc = "Accepted" });
        }

        private async Task UpdatePaymentStatus(MpesaResult result, PaymentStatus status)
        {
            try
            {
                if (string.IsNullOrEmpty(result.CheckoutRequestId))
                    return;

                var allPayments = await _unitOfWork.Repository<Payment>().GetAllAsync();
                var payment = allPayments.FirstOrDefault(p =>
                    p.ReferenceNo == result.CheckoutRequestId &&
                    p.PaymentMethod == PaymentMethod.Mpesa &&
                    p.Status == PaymentStatus.Pending);

                if (payment == null)
                {
                    _logger.LogWarning(
                        "Payment not found for CheckoutRequestId: {CheckoutId}",
                        result.CheckoutRequestId);
                    return;
                }

                payment.Status = status;
                payment.ResponseMessage = result.Message;

                if (status == PaymentStatus.Completed && !string.IsNullOrEmpty(result.MpesaReceiptNumber))
                {
                    // Update with M-Pesa receipt number
                    payment.ReferenceNo = result.MpesaReceiptNumber;

                    // Update the sales order
                    var salesOrder = await _unitOfWork.Repository<SalesOrder>()
                        .GetByIdAsync(payment.SalesOrderId);

                    if (salesOrder != null)
                    {
                        salesOrder.AmountPaid += payment.Amount;
                        await _unitOfWork.Repository<SalesOrder>().UpdateAsync(salesOrder);
                    }
                }

                await _unitOfWork.Repository<Payment>().UpdateAsync(payment);
                await _unitOfWork.Save(CancellationToken.None);

                _logger.LogInformation(
                    "Payment {PaymentId} updated to status {Status}",
                    payment.Id, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status");
            }
        }
    }

    public class TestStkRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Reference { get; set; }
    }
}

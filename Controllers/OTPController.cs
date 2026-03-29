using AstrolPOSAPI.Application.Features.OTP.Commands;
using AstrolPOSAPI.Application.Features.OTP.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astrol_POS_API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OTPController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OTPController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send OTP to phone number
        /// </summary>
        [HttpPost("send")]
        [ProducesResponseType(typeof(OTPDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendOTP([FromBody] SendOTPDto dto)
        {
            try
            {
                var result = await _mediator.Send(new SendOTPCommand
                {
                    PhoneNumber = dto.PhoneNumber,
                    Purpose = dto.Purpose
                });

                // Note: In production, don't return the OTP code to the client
                // For development, we're returning it so you can test
                return Ok(new
                {
                    message = "OTP sent successfully via SMS",
                    expiresAt = result.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verify OTP code
        /// </summary>
        [HttpPost("verify")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDto dto)
        {
            var isValid = await _mediator.Send(new VerifyOTPCommand
            {
                PhoneNumber = dto.PhoneNumber,
                OTPCode = dto.OTPCode,
                Purpose = dto.Purpose
            });

            if (!isValid)
                return BadRequest(new { message = "Invalid or expired OTP code" });

            return Ok(new { message = "OTP verified successfully", verified = true });
        }

        [HttpPost("resend")]
        [ProducesResponseType(typeof(OTPDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendOTP([FromBody] SendOTPDto dto)
        {
            return await SendOTP(dto);
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AstrolPOSAPI.Application.Features.Auth.DTOs;
using AstrolPOSAPI.Persistence.Contexts;
using AstrolPOSAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AstrolPOSAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AuthController(
            UserManager<AppUser> userManager,

            SignInManager<AppUser> signInManager,

            IConfiguration configuration,
            AppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public record RegisterRequest(string UserName, string Password, string Name, string? NationalID, string? StoreOfOperationId, string CompanyId);
        public record LoginRequest(string UserName, string Password);

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Name = request.Name,
                CompanyId = request.CompanyId,
                NationalID = request.NationalID,
                StoreOfOperationId = request.StoreOfOperationId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { message = "User registered." });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _dbContext.Users
                .Include(u => u.Company)
                .Include(u => u.UserStores)
                    .ThenInclude(us => us.Store)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Update login stats
            user.LastLoginAt = DateTimeOffset.UtcNow;
            user.LoginCount += 1;
            await _userManager.UpdateAsync(user);

            // Get company general settings for OTP flag
            var generalSettings = await _dbContext.GeneralSettings
                .FirstOrDefaultAsync(gs => gs.CompanyId == user.CompanyId);

            // Build store list from UserStores
            var stores = user.UserStores
                .Select(us => new StoreInfoDto
                {
                    Id = us.StoreId,
                    Name = us.Store?.Name ?? "Unknown",
                    IsPrimary = us.IsPrimary
                })
                .ToList();

            // Generate JWT token
            var token = await GenerateJwtToken(user);

            // Build enhanced response
            var response = new LoginResponseDto
            {
                Token = token,
                User = new LoginUserInfoDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name,
                    CompanyId = user.CompanyId,
                    CompanyName = user.Company?.Name,
                    Stores = stores,
                    HasOtp = generalSettings?.HasOtp ?? false,
                    PasswordChangeRequired = user.PasswordChangeRequired,
                    phoneNumber = user.PhoneNumber,
                    Role = user.Role,
                }
            };

            return Ok(response);
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // IMPORTANT: Also add role from AppUser.Role if Identity roles are not used
            if (!roles.Any() && !string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ========== PASSWORD MANAGEMENT ==========

        public record ForgotPasswordRequest(string Email);
        public record ResetPasswordRequest(string Email, string ResetToken, string NewPassword);

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ForgotPasswordResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist for security
                return Ok(new ForgotPasswordResponseDto
                {
                    Message = "If the email exists, a password reset token has been generated.",
                    TokenExpiry = DateTime.UtcNow.AddHours(1)
                });
            }

            // Generate password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: In production, send this via email instead of returning it
            // For now, return the token in the response
            return Ok(new ForgotPasswordResponseDto
            {
                Message = "Password reset token generated successfully. In production, this would be sent via email.",
                ResetToken = resetToken, // Remove this in production!
                TokenExpiry = DateTime.UtcNow.AddHours(1)
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Password reset failed",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            // Clear password change required flag if it was set
            user.PasswordChangeRequired = false;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Password reset successfully" });
        }

        // ========== USER CRUD ENDPOINTS ==========

        [HttpPost("users")]
        [Authorize(Roles = "Admin")] // Only admins can create users
        [ProducesResponseType(typeof(AstrolPOSAPI.Application.Features.User.DTOs.UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromServices] IMediator mediator, [FromServices] AppDbContext dbContext,

            [FromBody] AstrolPOSAPI.Application.Features.User.Commands.CreateUser.CreateUserCommand command)
        {
            try
            {
                var user = await mediator.Send(command);

                // Create UserStore junction records
                var storeIdsToAssign = command.StoreIds?.Any() == true
                    ? command.StoreIds
                    : (command.StoreOfOperationId != null
                        ? new List<string> { command.StoreOfOperationId }
                        : new List<string>());

                foreach (var storeId in storeIdsToAssign)
                {
                    dbContext.UserStores.Add(new UserStore
                    {
                        UserId = user.Id,

                        StoreId = storeId,
                        IsPrimary = storeId == command.StoreOfOperationId
                    });
                }

                await dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users")]
        [Authorize]
        [ProducesResponseType(typeof(List<AstrolPOSAPI.Application.Features.User.DTOs.UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers([FromServices] IMediator mediator, [FromQuery] string? companyId)
        {
            var users = await mediator.Send(new AstrolPOSAPI.Application.Features.User.Queries.GetAllUsersQuery
            {

                CompanyId = companyId


            });
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AstrolPOSAPI.Application.Features.User.DTOs.UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return NotFound(new { message = $"User with ID {id} not found" });

            return Ok(new AstrolPOSAPI.Application.Features.User.DTOs.UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                EmpNo = user.EmpNo,
                CompanyId = user.CompanyId,
                StoreOfOperationId = user.StoreOfOperationId,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                PasswordChangeRequired = user.PasswordChangeRequired,
                LastLoginAt = user.LastLoginAt?.UtcDateTime,
                LoginCount = user.LoginCount
            });
        }

        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromServices] IMediator mediator, string id)
        {
            try
            {
                await mediator.Send(new AstrolPOSAPI.Application.Features.User.Commands.DeleteUser.DeleteUserCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
        }

        [HttpGet("roles")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles([FromServices] IMediator mediator)
        {
            var roles = await mediator.Send(new AstrolPOSAPI.Application.Features.User.Queries.GetRoles.GetRolesQuery());
            return Ok(roles);
        }

        [HttpPut("users/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AstrolPOSAPI.Application.Features.User.DTOs.UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromServices] IMediator mediator, string id,
            [FromBody] AstrolPOSAPI.Application.Features.User.Commands.UpdateUser.UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                    return BadRequest(new { message = "ID mismatch between route and body" });

                var user = await mediator.Send(command);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("users/change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromServices] IMediator mediator,
            [FromBody] AstrolPOSAPI.Application.Features.User.Commands.ChangePassword.ChangePasswordCommand command)
        {
            try
            {
                await mediator.Send(command);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

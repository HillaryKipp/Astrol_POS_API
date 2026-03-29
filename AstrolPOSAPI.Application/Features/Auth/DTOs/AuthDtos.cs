namespace AstrolPOSAPI.Application.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public LoginUserInfoDto User { get; set; } = default!;
    }

    public class LoginUserInfoDto
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? CompanyName { get; set; }
        public string? phoneNumber { get; set; }
        public string? Role { get; set; }
        public List<StoreInfoDto> Stores { get; set; } = new();
        public bool HasOtp { get; set; }
        public bool PasswordChangeRequired { get; set; }
    }

    public class StoreInfoDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsPrimary { get; set; }
    }

    public class ForgotPasswordResponseDto
    {
        public string Message { get; set; } = default!;
        public string? ResetToken { get; set; } // Returned since we don't have email service yet
        public DateTime TokenExpiry { get; set; }
    }
}

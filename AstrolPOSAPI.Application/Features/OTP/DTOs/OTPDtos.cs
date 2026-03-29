namespace AstrolPOSAPI.Application.Features.OTP.DTOs
{
    public class OTPDto
    {
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Purpose { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        public int VerificationAttempts { get; set; }
    }

    public class SendOTPDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string Purpose { get; set; } = default!; // "Login", "PasswordReset", etc.
    }

    public class VerifyOTPDto
    {
        public string PhoneNumber { get; set; } = default!;
        public string OTPCode { get; set; } = default!;
        public string Purpose { get; set; } = default!;
    }
}

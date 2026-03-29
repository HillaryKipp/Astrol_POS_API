using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Identity;

namespace AstrolPOSAPI.Domain.Entities.Identity
{
    public class OTP : BaseAuditableEntity
    {
        public string UserId { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string OTPCode { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
        public int VerificationAttempts { get; set; }
        public DateTime? VerifiedAt { get; set; }

        // Purpose: "Login", "PasswordReset", "Registration", etc.
        public string Purpose { get; set; } = default!;

        // Navigation property
        public AppUser? User { get; set; }
    }
}

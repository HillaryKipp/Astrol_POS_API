namespace AstrolPOSAPI.Application.Features.User.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public string? EmpNo { get; set; }
        public string? Name { get; set; }
        public string? NationalID { get; set; }
        public string CompanyId { get; set; } = default!;
        public string? StoreOfOperationId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public bool PasswordChangeRequired { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public int LoginCount { get; set; }
    }

    public class CreateUserDto
    {
        public string UserName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? NationalID { get; set; }
        public string Password { get; set; } = default!;
        public string? Role { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string CompanyId { get; set; } = default!;
        public string StoreOfOperationId { get; set; } = default!;
    }

    public class UpdateUserDto
    {
        public string Id { get; set; } = default!;
        public string? Name { get; set; }
        public string? NationalID { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StoreOfOperationId { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class ChangePasswordDto
    {
        public string UserId { get; set; } = default!;
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}

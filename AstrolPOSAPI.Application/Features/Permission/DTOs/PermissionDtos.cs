namespace AstrolPOSAPI.Application.Features.Permission.DTOs
{
    public class PermissionDto
    {
        public string Id { get; set; } = default!;
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string ResourceName { get; set; } = default!;
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class CreatePermissionDto
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public string ResourceName { get; set; } = default!;
        public bool CanWrite { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class UpdatePermissionDto
    {
        public string Id { get; set; } = default!;
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}

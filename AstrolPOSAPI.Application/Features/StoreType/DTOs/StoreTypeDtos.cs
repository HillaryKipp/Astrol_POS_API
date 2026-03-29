namespace AstrolPOSAPI.Application.Features.StoreType.DTOs
{
    public class StoreTypeDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool HasOtp { get; set; }
        public string? CompanyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateStoreTypeDto
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool HasOtp { get; set; }
        public string? CompanyId { get; set; }
    }

    public class UpdateStoreTypeDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool HasOtp { get; set; }
        public string? CompanyId { get; set; }
    }
}

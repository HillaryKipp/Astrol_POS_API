namespace AstrolPOSAPI.Application.Features.NoSeries.DTOs
{
    public class NoSeriesDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateNoSeriesDto
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
    }

    public class UpdateNoSeriesDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? CurrentNo { get; set; }
    }
}

using AstrolPOSAPI.Domain.Common;

namespace AstrolPOSAPI.Domain.Entities.Audit
{
    public class AuditLog : BaseEntity
    {
        public string? UserId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? KeyValues { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
    }
}

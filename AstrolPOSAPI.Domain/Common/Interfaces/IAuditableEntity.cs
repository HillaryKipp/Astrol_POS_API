using System;

namespace AstrolPOSAPI.Domain.Common.Interfaces
{
    public interface IAuditableEntity
    {
        string? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string? DeletedBy { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}

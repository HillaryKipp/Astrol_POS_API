using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Core;

namespace AstrolPOSAPI.Domain.Entities.POS
{
    /// <summary>
    /// Represents a category for organizing POS items
    /// </summary>
    public class ItemCategory : BaseAuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        
        /// <summary>
        /// Optional parent category for hierarchical organization
        /// </summary>
        public string? ParentCategoryId { get; set; }
        public ItemCategory? ParentCategory { get; set; }
        
        public string CompanyId { get; set; } = string.Empty;
        public Company? Company { get; set; }
        
        public string StoreOfOperationId { get; set; } = string.Empty;
        public Store? StoreOfOperation { get; set; }
        
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}

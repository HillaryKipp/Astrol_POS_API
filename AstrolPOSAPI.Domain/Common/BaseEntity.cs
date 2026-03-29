using AstrolPOSAPI.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstrolPOSAPI.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        private readonly List<BaseEvent> _domainEvents = [];

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}

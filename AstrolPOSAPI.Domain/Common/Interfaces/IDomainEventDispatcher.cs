using System.Threading.Tasks;

namespace AstrolPOSAPI.Domain.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<BaseEntity> entities);
    }
}

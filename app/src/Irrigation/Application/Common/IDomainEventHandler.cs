using Irrigation.Domain.Common;
using Mediator;

namespace Irrigation.Application.Common
{
    public interface IDomainEventHandler<in T> : INotificationHandler<T> where T : IDomainEvent;
}

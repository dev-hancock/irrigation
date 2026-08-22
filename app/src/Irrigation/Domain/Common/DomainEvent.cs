using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Common;

public interface IDomainEvent : INotification, IIdentified;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
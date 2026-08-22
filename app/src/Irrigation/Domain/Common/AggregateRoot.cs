using Mediator;

namespace Irrigation.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events => _events;

    protected void Raise(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}


// proto

public sealed class OutboxMessageConsumer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

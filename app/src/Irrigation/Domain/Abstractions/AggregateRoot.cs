using Mediator;

namespace Irrigation.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<INotification> _events = [];

    public IReadOnlyCollection<INotification> Events => _events;

    protected void Raise(INotification @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}
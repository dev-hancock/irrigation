using Irrigation.Application.Common;
using Irrigation.Application.Valves.Events.Contracts;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveChangedHandler(IEventBus events) : 
    INotificationHandler<ValveStatusChangedEvent>,
    INotificationHandler<ValveNameChangedEvent>
{
    public async ValueTask Handle(ValveStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        await NotifyValveChanged(notification.Id.Value, cancellationToken);
    }

    public async ValueTask Handle(ValveNameChangedEvent notification, CancellationToken cancellationToken)
    {
        await NotifyValveChanged(notification.Id.Value, cancellationToken);
    }

    private Task NotifyValveChanged(Guid id, CancellationToken ct)
    {
        return events.Publish(
            new ValveChanged
            {
                Id = id
            }, ct);
    }
}
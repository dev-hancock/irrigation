using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveStatusChangedHandler(IEventBus events) : INotificationHandler<ValveStatusChangedEvent>
{
    public async ValueTask Handle(ValveStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, Status = notification.Status
            }, cancellationToken);
    }
}
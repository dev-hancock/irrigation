using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveClosedHandler(IEventBus events) : INotificationHandler<ValveClosed>
{
    public async ValueTask Handle(ValveClosed notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, Status = ValveStatus.Closed
            }, cancellationToken);
    }
}
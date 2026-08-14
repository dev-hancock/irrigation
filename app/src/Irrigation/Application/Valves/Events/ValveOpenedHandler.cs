using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public sealed class ValveOpenedHandler(IEventBus events) : INotificationHandler<ValveOpened>
{
    public async ValueTask Handle(ValveOpened notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, Status = ValveStatus.Opened
            }, cancellationToken);
    }
}
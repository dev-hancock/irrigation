using Irrigation.Application.Common;
using Irrigation.Application.Extensions;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveOpeningHandler(IValveService valves, IEventBus events) : INotificationHandler<ValveOpeningEvent>
{
    public async ValueTask Handle(ValveOpeningEvent notification, CancellationToken cancellationToken)
    {
        var result = await valves.Open(notification.Id, cancellationToken);

        result.ThrowIfError();

        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, Status = ValveStatus.Opening
            }, cancellationToken);
    }
}
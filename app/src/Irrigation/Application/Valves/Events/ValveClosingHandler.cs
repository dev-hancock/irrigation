using Irrigation.Application.Common;
using Irrigation.Application.Extensions;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveClosingHandler(IValveService valves, IEventBus events) : INotificationHandler<ValveClosingEvent>
{
    public async ValueTask Handle(ValveClosingEvent notification, CancellationToken cancellationToken)
    {
        var result = await valves.Close(notification.Id, cancellationToken);

        result.ThrowIfError();

        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, Status = ValveStatus.Closing
            }, cancellationToken);
    }
}
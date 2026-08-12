using Irrigation.Application.Common;
using Irrigation.Application.Extensions;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveClosingHandler(IValveService valves, IEventBus events) : INotificationHandler<ValveClosing>
{
    public async ValueTask Handle(ValveClosing notification, CancellationToken cancellationToken)
    {
        var result = await valves.Close(notification.Id, cancellationToken);

        result.ThrowIfError();

        await events.Publish(
            new ValveStateChanged
            {
                Id = notification.Id, State = ValveState.Closing
            }, cancellationToken);
    }
}
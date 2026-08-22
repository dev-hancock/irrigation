using Irrigation.Application.Common;
using Irrigation.Application.Valves.Events.Outbound;
using Irrigation.Domain.Valves.Events;
using Mediator;

namespace Irrigation.Application.Valves.Events.Domain;

public class ValveClosedHandler(IEventBus events) : IDomainEventHandler<ValveClosedEvent>
{
    public async ValueTask Handle(ValveClosedEvent notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ValveChanged
            {
                Id = notification.Id.Value
            }, cancellationToken);
    }
}
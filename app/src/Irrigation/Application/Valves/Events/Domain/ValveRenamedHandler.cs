using Irrigation.Application.Common;
using Irrigation.Application.Valves.Events.Outbound;
using Irrigation.Domain.Valves.Events;
using Mediator;

namespace Irrigation.Application.Valves.Events.Domain;

public class ValveRenamedHandler(IEventBus events) : IDomainEventHandler<ValveRenamedEvent>
{
    public async ValueTask Handle(ValveRenamedEvent notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ValveChanged
            {
                Id = notification.Id
            }, cancellationToken);
    }
}
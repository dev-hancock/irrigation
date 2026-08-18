using Irrigation.Application.Activities.Events.Outbound;
using Irrigation.Application.Common;
using Irrigation.Domain.Activities.Events;
using Mediator;

namespace Irrigation.Application.Activities.Events.Domain;

public class ActivityCreatedHandler(IEventBus events) : INotificationHandler<ActivityCreatedEvent>
{
    public async ValueTask Handle(ActivityCreatedEvent notification, CancellationToken cancellationToken)
    {
        await events.Publish(
            new ActivityCreated
            {
                //Id = notification.Id.Value
            }, cancellationToken);
    }
}
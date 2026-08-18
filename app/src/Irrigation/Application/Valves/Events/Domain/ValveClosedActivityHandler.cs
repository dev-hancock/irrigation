using Irrigation.Application.Activities.Abstractions;
using Irrigation.Application.Extensions;
using Irrigation.Application.Valves.Activities;
using Irrigation.Domain.Valves.Events;
using Mediator;

namespace Irrigation.Application.Valves.Events.Domain;

public class ValveClosedActivityHandler(IActivityWriter activity) : INotificationHandler<ValveClosedEvent>
{
    public async ValueTask Handle(ValveClosedEvent notification, CancellationToken cancellationToken)
    {
        var result = await activity.Write(
            ValveActivity.Closed,
            ValveActivity.Category,
            notification.Origin,
            new ValveActivityData
            {
                Id = notification.Id.Value, Name = notification.Name
            },
            cancellationToken
        );

        result.ThrowIfError();
    }
}
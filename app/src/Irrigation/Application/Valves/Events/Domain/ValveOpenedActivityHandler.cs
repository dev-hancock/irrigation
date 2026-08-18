using Irrigation.Application.Activities.Abstractions;
using Irrigation.Application.Extensions;
using Irrigation.Application.Valves.Activities;
using Irrigation.Domain.Valves.Events;
using Mediator;

namespace Irrigation.Application.Valves.Events.Domain;

public class ValveOpenedActivityHandler(IActivityWriter activity) : INotificationHandler<ValveOpenedEvent>
{
    public async ValueTask Handle(ValveOpenedEvent notification, CancellationToken cancellationToken)
    {
        var result = await activity.Write(
            ValveActivity.Opened,
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
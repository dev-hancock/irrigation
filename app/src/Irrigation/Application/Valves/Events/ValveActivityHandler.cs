using Irrigation.Application.Activities.Abstractions;
using Mediator;

namespace Irrigation.Application.Valves.Events
{
    public sealed record ValveActivityEvent : INotification
    {

    }

    public class ValveActivityHandler(IActivityWriter activites): INotificationHandler<ValveActivityEvent>
    {
        public ValueTask Handle(ValveActivityEvent notification, CancellationToken cancellationToken)
        {
            



            return ValueTask.CompletedTask;
        }
    }
}

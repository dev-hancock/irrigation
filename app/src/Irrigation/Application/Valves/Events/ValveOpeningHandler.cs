using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events
{
    public class ValveOpeningHandler(IValveService valves, IEventBus events) : INotificationHandler<ValveOpening>
    {
        public async ValueTask Handle(ValveOpening notification, CancellationToken ct = default)
        {
            await valves.Open(notification.Id, ct);

            await events.Publish(
                new ValveStateChanged
                {
                    Id = notification.Id, 
                    State = ValveState.Opening
                }, ct);

        }
    }
}

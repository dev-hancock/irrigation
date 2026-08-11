using Irrigation.Application.Common;
using Irrigation.Domain.Common;
using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves.Events
{
    public sealed class ValveOpenedHandler : INotificationHandler<ValveOpened>
    {
        public Task Handle(ValveOpened notification)
        {
            throw new NotImplementedException();
        }
    }
}

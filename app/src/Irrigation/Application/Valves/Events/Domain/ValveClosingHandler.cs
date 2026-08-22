using Irrigation.Application.Common;
using Irrigation.Application.Extensions;
using Irrigation.Application.Valves.Events.Outbound;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Devices.Specifications;
using Irrigation.Domain.Valves.Events;
using Mediator;

namespace Irrigation.Application.Valves.Events.Domain;

public class ValveClosingHandler(IRepository<Device> devices, IValveController controller, IEventBus events)
    : IDomainEventHandler<ValveClosingEvent>
{
    public async ValueTask Handle(ValveClosingEvent notification, CancellationToken cancellationToken)
    {
        var spec = new DeviceSpec(notification.DeviceId);

        var device = await devices.FirstOrDefaultAsync(spec, cancellationToken);

        if (device is null)
        {
            return;
        }

        var result = await controller.Close(notification.Index, device.HardwareId, cancellationToken);

        result.ThrowIfError();

        await events.Publish(
            new ValveChanged
            {
                Id = notification.Id
            }, cancellationToken);
    }
}
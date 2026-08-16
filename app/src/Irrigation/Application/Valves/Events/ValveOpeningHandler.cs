using Irrigation.Application.Common;
using Irrigation.Application.Extensions;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public class ValveOpeningHandler(IRepository<Device> devices, IValveService valves, IEventBus events)
    : INotificationHandler<ValveOpeningEvent>
{
    public async ValueTask Handle(ValveOpeningEvent notification, CancellationToken cancellationToken)
    {
        var device = await devices.FirstOrDefaultAsync(
            new DeviceSpec(notification.DeviceId),
            cancellationToken);

        if (device is null)
        {
            throw new InvalidOperationException($"Device '{notification.DeviceId}' not found.");
        }

        var result = await valves.Open(notification.Index, device.HardwareId, cancellationToken);

        result.ThrowIfError();

        await events.Publish(
            new ValveChanged
            {
                Id = notification.Id
            }, cancellationToken);
    }
}
using Irrigation.Application.Common;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Devices.Specifications;
using Mediator;

namespace Irrigation.Application.Devices.Events.Inbound;

public class UpdateDeviceHandler(IRepository<Device> repo) : INotificationHandler<UpdateDeviceEvent>
{
    public async ValueTask Handle(UpdateDeviceEvent notification, CancellationToken cancellationToken)
    {
        var spec = new DeviceSpec(notification.Id);

        var device = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (device is null)
        {
            device = Device.Create(
                notification.Id,
                notification.Firmware,
                notification.Model,
                notification.Version);

            await repo.AddAsync(device, cancellationToken);
        }
        else
        {
            device.Update(
                notification.Firmware,
                notification.Model,
                notification.Version);
        }

        await repo.SaveChangesAsync(cancellationToken);
    }
}
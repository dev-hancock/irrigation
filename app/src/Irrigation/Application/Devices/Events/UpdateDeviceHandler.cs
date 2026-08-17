using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Mediator;

namespace Irrigation.Application.Devices.Events;

public class UpdateDeviceEvent : INotification
{
    public required HardwareId Id { get; set; }

    public required string Firmware { get; set; }

    public required string Model { get; set; }

    public required string Version { get; set; }
}

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
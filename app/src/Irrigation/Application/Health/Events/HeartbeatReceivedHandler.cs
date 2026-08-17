using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Mediator;

namespace Irrigation.Application.Health.Events;

public class HeartbeatReceivedEvent : INotification
{
    public HardwareId Id { get; set; }
}

public class HeartbeatReceivedHandler(IRepository<Device> repo) : INotificationHandler<HeartbeatReceivedEvent>
{
    public async ValueTask Handle(HeartbeatReceivedEvent notification, CancellationToken cancellationToken)
    {
        var spec = new DeviceSpec(notification.Id);

        var device = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (device is null)
        {
            throw new InvalidOperationException($"Device '{notification.Id}' not found.");
        }

        device.Heartbeat();

        await repo.SaveChangesAsync(cancellationToken);
    }
}
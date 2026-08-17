using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events;

public sealed record UpdateValveEvent : INotification
{
    public required HardwareId Device { get; set; }

    public required int Index { get; set; }

    public required ValveStatus Status { get; set; }
}

public class UpdateValveHandler(IUnitOfWork uow, ILogger<UpdateValveHandler> logger) : INotificationHandler<UpdateValveEvent>
{
    public async ValueTask Handle(UpdateValveEvent notification, CancellationToken cancellationToken)
    {
        var spec = new DeviceSpec(notification.Device);

        var device = await uow.Devices.FirstOrDefaultAsync(spec, cancellationToken);

        if (device is null)
        {
            throw new InvalidOperationException($"Device '{notification.Device.Value}' not found.");
        }

        var valve = await uow.Valves.FirstOrDefaultAsync(
            new ValveSpec(notification.Index, device.Id),
            cancellationToken);

        if (valve is null)
        {
            valve = Valve.Create(
                device.Id,
                notification.Index,
                notification.Status
            );

            await uow.Valves.AddAsync(valve, cancellationToken);
        }
        else
        {
            valve.SetStatus(notification.Status);
        }

        logger.LogInformation($"Valve '{device.HardwareId.Value}:{valve.Index}' is {valve.Status}");

        await uow.SaveChangesAsync(cancellationToken);
    }
}
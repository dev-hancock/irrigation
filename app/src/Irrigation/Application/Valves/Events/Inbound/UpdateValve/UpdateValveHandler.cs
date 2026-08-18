using Irrigation.Application.Common;
using Irrigation.Domain.Devices.Specifications;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Events.Inbound.UpdateValve;

public class UpdateValveHandler(IUnitOfWork uow, ILogger<UpdateValveHandler> logger) : INotificationHandler<UpdateValveEvent>
{
    public async ValueTask Handle(UpdateValveEvent notification, CancellationToken cancellationToken)
    {
        var device = await uow.Devices.FirstOrDefaultAsync(
            new DeviceSpec(notification.Device),
            cancellationToken);

        if (device is null)
        {
            throw new InvalidOperationException($"Device '{notification.Device.Value}' not found.");
        }

        var valve = await uow.Valves.FirstOrDefaultAsync(
            new ValveSpec(device.Id, notification.Index),
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
            switch (notification.Status)
            {
                case ValveStatus.Open:
                {
                    valve.Opened();
                    break;
                }
                case ValveStatus.Closed:
                {
                    valve.Closed();
                    break;
                }
                case ValveStatus.Opening:
                case ValveStatus.Closing:
                default:
                    break;
            }
        }

        logger.LogInformation($"Valve '{device.HardwareId.Value}:{valve.Index}' is {valve.Status}");

        await uow.SaveChangesAsync(cancellationToken);
    }
}
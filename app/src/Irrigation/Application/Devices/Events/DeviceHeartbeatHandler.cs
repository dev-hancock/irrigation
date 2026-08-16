using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Mediator;

namespace Irrigation.Application.Devices.Events;

public class DeviceHeartbeatCommand : IRequest<ErrorOr<Success>>
{
    public string Id { get; set; }
}

public class DeviceHeartbeatHandler(IRepository<Device> repo) : IRequestHandler<DeviceHeartbeatCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(DeviceHeartbeatCommand request, CancellationToken cancellationToken)
    {
        var device = await repo.FirstOrDefaultAsync(
            new DeviceSpec(HardwareId.From(request.Id)),
            cancellationToken);

        if (device is null)
        {
            return Error.NotFound("Device.NotFound", $"Device with id '{request.Id}' not found.");
        }

        device.Heartbeat();

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
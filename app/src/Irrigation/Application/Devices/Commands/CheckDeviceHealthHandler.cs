using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Mediator;

namespace Irrigation.Application.Devices.Commands;

public class CheckDeviceHealthCommand : IRequest<ErrorOr<Success>>;

public class CheckDeviceHealthHandler(IRepository<Device> repo) : IRequestHandler<CheckDeviceHealthCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CheckDeviceHealthCommand request, CancellationToken cancellationToken)
    {
        var devices = await repo.ListAsync(
            new DeviceNotSeenSinceSpec(DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(30))),
            cancellationToken);

        foreach (var device in devices)
        {
            device.Offline();
        }

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
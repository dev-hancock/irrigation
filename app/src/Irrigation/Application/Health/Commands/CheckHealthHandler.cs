using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Devices.Specifications;
using Mediator;

namespace Irrigation.Application.Health.Commands;

public class CheckHealthCommand : IRequest<ErrorOr<Success>>;

public class CheckHealthHandler(IRepository<Device> repo) : IRequestHandler<CheckHealthCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CheckHealthCommand request, CancellationToken cancellationToken)
    {
        var spec = new DeviceNotSeenSinceSpec(DateTimeOffset.UtcNow.Subtract(TimeSpan.FromSeconds(30)));

        var devices = await repo.ListAsync(spec, cancellationToken);

        foreach (var device in devices)
        {
            device.Offline();
        }

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Mediator;

namespace Irrigation.Application.Devices.Commands;

public class UpdateDeviceCommand : IRequest<ErrorOr<Success>>
{
    public required string Id { get; set; }

    public required string Firmware { get; set; }

    public required string Model { get; set; }

    public required string Version { get; set; }
}

public class UpdateDeviceHandler(IRepository<Device> repo) : IRequestHandler<UpdateDeviceCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await repo.FirstOrDefaultAsync(
            new DeviceSpec(HardwareId.From(request.Id)),
            cancellationToken);

        if (device is null)
        {
            device = Device.Create(
                HardwareId.From(request.Id),
                request.Firmware,
                request.Model,
                request.Version);

            await repo.AddAsync(device, cancellationToken);
        }
        else
        {
            device.Update(
                request.Firmware,
                request.Model,
                request.Version);
        }

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
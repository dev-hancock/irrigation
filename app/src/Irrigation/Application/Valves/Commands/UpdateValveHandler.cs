using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record UpdateValveCommand : IRequest<ErrorOr<Success>>
{
    public required string Device { get; set; }

    public required string Id { get; set; }

    public required string Status { get; set; }
}

public class UpdateValveHandler(IRepository<Valve> valves, IRepository<Device> devices)
    : IRequestHandler<UpdateValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(UpdateValveCommand request, CancellationToken cancellationToken)
    {
        var valve = await valves.FirstOrDefaultAsync(
            new GetValveSpec(HardwareId.From(request.Id)),
            cancellationToken);

        if (!Enum.TryParse<ValveStatus>(request.Status, true, out var status))
        {
            return Error.Failure("Valve.InvalidState", $"Invalid valve status '{request.Status}'.");
        }

        if (valve is null)
        {
            var device = await devices.FirstOrDefaultAsync(
                new GetDeviceSpec(HardwareId.From(request.Device)),
                cancellationToken);

            if (device is null)
            {
                return Error.NotFound("Device.NotFound", $"Device with id '{request.Device}' not found.");
            }

            valve = Valve.Create(
                device.Id,
                HardwareId.From(request.Id),
                status
            );

            await valves.AddAsync(valve, cancellationToken);
        }
        else
        {
            valve.SetStatus(status);
        }

        await valves.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
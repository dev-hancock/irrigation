using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record UpdateValveCommand : IRequest<ErrorOr<Success>>
{
    public required string Device { get; set; }

    public required int Index { get; set; }

    public required string Status { get; set; }
}

public class UpdateValveHandler(IUnitOfWork uow, ILogger<UpdateValveHandler> logger)
    : IRequestHandler<UpdateValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(UpdateValveCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ValveStatus>(request.Status, true, out var status))
        {
            return Error.Failure("Valve.InvalidState", $"Invalid valve status '{request.Status}'.");
        }

        var device = await uow.Devices.FirstOrDefaultAsync(
            new DeviceSpec(HardwareId.From(request.Device)),
            cancellationToken);

        if (device is null)
        {
            return Error.NotFound("Device.NotFound", $"Device with id '{request.Device}' not found.");
        }

        var valve = await uow.Valves.FirstOrDefaultAsync(
            new ValveSpec(request.Index, device.Id),
            cancellationToken);

        if (valve is null)
        {
            valve = Valve.Create(
                device.Id,
                request.Index,
                status
            );

            await uow.Valves.AddAsync(valve, cancellationToken);
        }
        else
        {
            valve.SetStatus(status);
        }

        logger.LogInformation($"Valve '{device.HardwareId.Value}:{valve.Index}' is {valve.Status}");

        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
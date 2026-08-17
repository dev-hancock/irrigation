using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record UpdateValveCommand : IRequest<ErrorOr<Success>>
{
    public required HardwareId Device { get; set; }

    public required int Index { get; set; }

    public required ValveStatus Status { get; set; }
}

public class UpdateValveHandler(IUnitOfWork uow, ILogger<UpdateValveHandler> logger)
    : IRequestHandler<UpdateValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(UpdateValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new DeviceSpec(request.Device);

        var device = await uow.Devices.FirstOrDefaultAsync(spec, cancellationToken);

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
                request.Status
            );

            await uow.Valves.AddAsync(valve, cancellationToken);
        }
        else
        {
            valve.SetStatus(request.Status);
        }

        logger.LogInformation($"Valve '{device.HardwareId.Value}:{valve.Index}' is {valve.Status}");

        await uow.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
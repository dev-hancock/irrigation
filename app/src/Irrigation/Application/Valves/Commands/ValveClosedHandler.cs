using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record ValveClosedCommand : IRequest<ErrorOr<Success>>
{
    public required string Device { get; set; }

    public required string Id { get; set; }
}

public class ValveClosedHandler(IRepository<Valve> repo) : IRequestHandler<ValveClosedCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(ValveClosedCommand request, CancellationToken ct = default)
    {
        var spec = new GetValveSpec(request.Device, request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, ct);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with device '{request.Device}' and id '{request.Id}' not found.");
        }

        valve.Closed();

        await repo.SaveChangesAsync(ct);

        return Result.Success;
    }
}
using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record CloseValveCommand : IRequest<ErrorOr<Success>>
{
    public string Device { get; set; }

    public string Id { get; set; }
}

public sealed class CloseValveHandler(IRepository<Valve> repo) : IRequestHandler<CloseValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CloseValveCommand request, CancellationToken ct = default)
    {
        var spec = new GetValveSpec(request.Device, request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, ct);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with device '{request.Device}' and id '{request.Id}' not found.");
        }

        valve.Open();

        await repo.SaveChangesAsync(ct);

        return Result.Success;
    }
}
using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record ResetValveCommand : IRequest<ErrorOr<Success>>
{
    public string? Device { get; set; }
}

public sealed class ResetValveHandler(IRepository<Valve> repo) : IRequestHandler<ResetValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(ResetValveCommand request, CancellationToken ct = default)
    {
        var spec = new GetValvesSpec(request.Device);

        var valves = await repo.ListAsync(spec, ct);

        foreach (var valve in valves)
        {
            valve.Close();
        }

        await repo.SaveChangesAsync(ct);

        return Result.Success;
    }
}
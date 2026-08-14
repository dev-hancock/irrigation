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
    public async ValueTask<ErrorOr<Success>> Handle(ResetValveCommand request, CancellationToken cancellationToken)
    {
        var valves = await repo.ListAsync(
            new GetValvesSpec(request.Device),
            cancellationToken);

        foreach (var valve in valves)
        {
            valve.Close();
        }

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
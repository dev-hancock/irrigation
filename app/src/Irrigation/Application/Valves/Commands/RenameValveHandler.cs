using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record RenameValveCommand : IRequest<ErrorOr<Success>>
{
    public required ValveId Id { get; set; }

    public required string Name { get; set; }
}

public class RenameValveHandler(IRepository<Valve> repo) : IRequestHandler<RenameValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(RenameValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new ValveSpec(request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id.Value}' not found.");
        }

        valve.Rename(request.Name);

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
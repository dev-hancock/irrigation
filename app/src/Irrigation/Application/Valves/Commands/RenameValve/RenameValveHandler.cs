using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Commands.RenameValve;

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
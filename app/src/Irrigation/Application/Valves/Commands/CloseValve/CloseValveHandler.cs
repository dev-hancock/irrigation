using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Application.Common.Sagas;
using Irrigation.Application.Valves.Sagas;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Commands.CloseValve;

public sealed class CloseValveHandler(IRepository<Valve> repo, ISagaStore sagas)
    : IRequestHandler<CloseValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CloseValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new ValveSpec(request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id.Value}' not found.");
        }

        var result =  valve.Close();

        if (!result)
        {
            return Result.Success;
        }

        await sagas.Start(
            new ValveOperationState
            {
                ValveId = valve.Id,
                Target = ValveStatus.Closed,
                Origin = ActionOrigin.Manual
            },
            cancellationToken);

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
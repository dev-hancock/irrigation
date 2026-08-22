using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Application.Common.Sagas;
using Irrigation.Application.Valves.Sagas;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Commands.OpenValve;

public sealed class OpenValveHandler(IRepository<Valve> valves, ISagaStore sagas)
    : IRequestHandler<OpenValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(OpenValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new ValveSpec(request.Id);

        var valve = await valves.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id.Value}' not found.");
        }

        var result = valve.Open();

        if (!result)
        {
            return Result.Success;
        }

        await sagas.Start(
            new ValveOperationState
            {
                ValveId = valve.Id, Target = ValveStatus.Open, Origin = ActionOrigin.Manual
            },
            cancellationToken);

        await valves.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
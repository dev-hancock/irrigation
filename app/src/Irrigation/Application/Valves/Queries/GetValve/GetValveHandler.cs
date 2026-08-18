using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Queries.GetValve;

public class GetValveHandler(IRepository<Valve> repo) : IRequestHandler<GetValveQuery, ErrorOr<ValveModel>>
{
    public async ValueTask<ErrorOr<ValveModel>> Handle(GetValveQuery request, CancellationToken cancellationToken)
    {
        var spec = new ValveReadOnlySpec(request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id.Value}' not found.");
        }

        return ValveModel.From(valve);
    }
}
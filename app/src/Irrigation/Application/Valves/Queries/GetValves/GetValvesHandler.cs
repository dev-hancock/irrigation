using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;
using Mediator;

namespace Irrigation.Application.Valves.Queries.GetValves;

public class GetValvesHandler(IRepository<Valve> repo) : IRequestHandler<GetValvesQuery, ErrorOr<ValveModel[]>>
{
    public async ValueTask<ErrorOr<ValveModel[]>> Handle(GetValvesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ValvesReadOnlySpec(request.Device);

        var valves = await repo.ListAsync(spec, cancellationToken);

        return valves
            .Select(ValveModel.From)
            .ToArray();
    }
}
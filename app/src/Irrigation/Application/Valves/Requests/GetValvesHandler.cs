using ErrorOr;
using Irrigation.Application.Valves.Requests.Contracts;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Requests;

public class GetValvesRequest : IRequest<ErrorOr<ValveDto[]>>
{
    public DeviceId? Device { get; set; }
}

public class GetValvesHandler(IRepository<Valve> repo) : IRequestHandler<GetValvesRequest, ErrorOr<ValveDto[]>>
{
    public async ValueTask<ErrorOr<ValveDto[]>> Handle(GetValvesRequest request, CancellationToken cancellationToken)
    {
        var spec = new ValvesReadOnlySpec(request.Device);

        var valves = await repo.ListAsync(spec, cancellationToken);

        return valves
            .Select(ValveDto.From)
            .ToArray();
    }
}
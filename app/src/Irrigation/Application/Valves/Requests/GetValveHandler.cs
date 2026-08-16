using ErrorOr;
using Irrigation.Application.Valves.Requests.Contracts;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Requests;

public class GetValveRequest : IRequest<ErrorOr<ValveDto>>
{
    public Guid Id { get; set; }
}

public class GetValveHandler(IRepository<Valve> repo) : IRequestHandler<GetValveRequest, ErrorOr<ValveDto>>
{
    public async ValueTask<ErrorOr<ValveDto>> Handle(GetValveRequest request, CancellationToken cancellationToken)
    {
        var valve = await repo.FirstOrDefaultAsync(
            new ValveReadOnlySpec(ValveId.From(request.Id)),
            cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id}' not found.");
        }

        return ValveDto.From(valve);
    }
}
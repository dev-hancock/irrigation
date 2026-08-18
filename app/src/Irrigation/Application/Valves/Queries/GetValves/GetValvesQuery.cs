using ErrorOr;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Valves.Queries.GetValves;

public class GetValvesQuery : IRequest<ErrorOr<ValveModel[]>>
{
    public DeviceId? Device { get; set; }
}
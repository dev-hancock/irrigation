using ErrorOr;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Valves.Queries.GetValve;

public class GetValveQuery : IRequest<ErrorOr<ValveModel>>
{
    public ValveId Id { get; set; }
}
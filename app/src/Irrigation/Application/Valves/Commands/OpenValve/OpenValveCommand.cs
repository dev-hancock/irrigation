using ErrorOr;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Valves.Commands.OpenValve;

public sealed record OpenValveCommand : IRequest<ErrorOr<Success>>
{
    public required ValveId Id { get; set; }
}
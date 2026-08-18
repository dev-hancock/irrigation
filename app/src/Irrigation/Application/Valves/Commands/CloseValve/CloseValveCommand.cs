using ErrorOr;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Valves.Commands.CloseValve;

public sealed record CloseValveCommand : IRequest<ErrorOr<Success>>
{
    public required ValveId Id { get; set; }
}
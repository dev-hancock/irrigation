using ErrorOr;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Valves.Commands.RenameValve;

public sealed record RenameValveCommand : IRequest<ErrorOr<Success>>
{
    public required ValveId Id { get; set; }

    public required string Name { get; set; }
}
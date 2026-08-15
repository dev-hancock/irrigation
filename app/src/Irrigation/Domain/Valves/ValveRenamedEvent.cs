using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveRenamedEvent : INotification
{
    public ValveId Id { get; init; }

    public string Name { get; init; }
}
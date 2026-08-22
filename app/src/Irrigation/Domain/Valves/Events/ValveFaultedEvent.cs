using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves.Events;

public sealed record ValveFaultedEvent : DomainEvent
{
    public required ValveId Id { get; init; }

    public required string Name { get; init; }

    public required int Index { get; init; }

    public required DeviceId DeviceId { get; init; }
}
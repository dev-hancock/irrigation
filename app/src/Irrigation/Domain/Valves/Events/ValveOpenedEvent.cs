using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves.Events;

public sealed record ValveOpenedEvent : INotification
{
    public required ValveId Id { get; init; }

    public required string Name { get; init; }

    public required int Index { get; init; }

    public required DeviceId DeviceId { get; init; }

    public required ActivityOrigin Origin { get; init; }
}
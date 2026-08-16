using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveOpeningEvent : INotification
{
    public required ValveId Id { get; init; }

    public required int Index { get; init; }

    public required DeviceId DeviceId { get; init; }
}
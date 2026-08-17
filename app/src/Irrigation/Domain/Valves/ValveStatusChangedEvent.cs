using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveStatusChangedEvent : INotification
{
    public required ValveId Id { get; init; }

    public required ValveStatus Status { get; init; }
}


public sealed record ValveNameChangedEvent : INotification
{
    public required ValveId Id { get; init; }

    public required string OldName { get; init; }

    public required string NewName { get; init; }
}
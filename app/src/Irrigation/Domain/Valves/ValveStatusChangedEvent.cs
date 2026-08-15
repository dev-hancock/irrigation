using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveStatusChangedEvent : INotification
{
    public ValveId Id { get; init; }

    public ValveStatus Status { get; init; }
}
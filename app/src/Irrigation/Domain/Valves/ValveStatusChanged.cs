using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveStatusChanged : INotification
{
    public ValveId Id { get; init; }

    public ValveStatus Status { get; init; }
}
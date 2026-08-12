using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveOpening : INotification
{
    public ValveId Id { get; internal set; }
}
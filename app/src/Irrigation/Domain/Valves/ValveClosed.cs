using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveClosed : INotification
{
    public ValveId Id { get; internal set; }
}
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveClosingEvent : INotification
{
    public ValveId Id { get; internal set; }
}
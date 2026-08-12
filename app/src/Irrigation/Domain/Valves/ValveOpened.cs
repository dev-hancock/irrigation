using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveOpened : INotification
{
    public ValveId Id { get; internal set; }
}
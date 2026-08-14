using Mediator;

namespace Irrigation.Domain.Valves;

public sealed record ValveRenamed : INotification
{
    public ValveId Id { get; init; }

    public string Name { get; init; }
}
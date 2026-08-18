namespace Irrigation.Application.Valves.Events.Outbound;

public sealed record ValveChanged
{
    public Guid Id { get; init; }
}
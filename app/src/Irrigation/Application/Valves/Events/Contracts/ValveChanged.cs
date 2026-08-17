namespace Irrigation.Application.Valves.Events.Contracts;

public sealed record ValveChanged
{
    public Guid Id { get; init; }
}
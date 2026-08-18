namespace Irrigation.Application.Valves.Activities;

public sealed record ValveActivityData
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
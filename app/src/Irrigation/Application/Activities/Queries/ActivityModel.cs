namespace Irrigation.Application.Activities.Queries;

public sealed record ActivityModel
{
    public required DateTimeOffset Timestamp { get; init; }

    public required string[] Arguments { get; init; }

    public required string Type { get; init; }

    public required string Category { get; init; }

    public required string Origin { get; init; }
}
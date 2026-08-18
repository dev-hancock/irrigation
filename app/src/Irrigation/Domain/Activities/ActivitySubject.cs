namespace Irrigation.Domain.Activities;

public sealed record ActivitySubject
{
    public required string Type { get; init; }

    public required Guid Id { get; init; }
}
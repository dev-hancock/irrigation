namespace Irrigation.Infrastructure.Sagas;

public sealed record SagaInstance
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Type { get; init; }

    public required string Data { get; set; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? NextAttemptAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public DateTimeOffset? FailedAt { get; set; }

    public string? Error { get; set; }

    public int Attempts { get; set; }

    public bool Completed => CompletedAt.HasValue;

    public bool Failed => FailedAt.HasValue;

    public bool Resolved => Completed || Failed;
}
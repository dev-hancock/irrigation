namespace Irrigation.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Type { get; init; }

    public required string Data { get; set; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ProcessedAt { get; set; }

    public string? Error { get; set; }

    public int Attempts { get; set; }

    public bool Processed => ProcessedAt.HasValue;
}
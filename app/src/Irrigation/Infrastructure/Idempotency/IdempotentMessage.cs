namespace Irrigation.Infrastructure.Idempotency;

public sealed record IdempotentMessage
{
    public required Guid MessageId { get; init; }

    public required string Handler { get; init; }

    public DateTimeOffset ProcessedAt { get; init; } = DateTimeOffset.UtcNow;
}
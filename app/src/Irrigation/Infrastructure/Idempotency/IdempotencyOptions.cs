namespace Irrigation.Infrastructure.Idempotency;

public sealed class IdempotencyOptions
{
    public const string Section = "Idempotency";

    public TimeSpan Retention { get; set; } = TimeSpan.FromDays(7);

    public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromHours(1);
}
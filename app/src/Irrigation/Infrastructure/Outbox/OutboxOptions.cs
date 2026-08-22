namespace Irrigation.Infrastructure.Outbox;

public sealed class OutboxOptions
{
    public const string Section = "Outbox";

    public TimeSpan Retention { get; set; } = TimeSpan.FromDays(7);
}
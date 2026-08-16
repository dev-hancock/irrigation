namespace Irrigation.Infrastructure.Health;

public class HealthOptions
{
    public const string Section = "Device";

    public TimeSpan Heartbeat { get; set; } = TimeSpan.FromSeconds(10);
}
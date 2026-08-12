namespace Irrigation.Infrastructure.Mqtt;

public sealed record Message
{
    public string Device { get; set; }

    public string Topic { get; set; }

    public string Route { get; set; }

    public string Payload { get; set; }
}
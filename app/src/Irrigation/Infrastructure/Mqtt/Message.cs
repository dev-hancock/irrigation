namespace Irrigation.Infrastructure.Mqtt;

public sealed record Message
{
    public string Device => Topic.Device;

    public required Topic Topic { get; init; }

    public required string Payload { get; init; }

    public string this[int index] => Topic[index];
}
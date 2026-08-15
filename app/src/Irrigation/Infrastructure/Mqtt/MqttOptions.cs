namespace Irrigation.Infrastructure.Mqtt;

public class MqttOptions
{
    public const string Section = "Mqtt";

    public string ClientId { get; init; }

    public string Host { get; init; }

    public int Port { get; init; }

    public string Username { get; init; }

    public string Password { get; init; }
}
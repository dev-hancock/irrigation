namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMqttPublisher
{
    Task Publish(
        string topic,
        object payload,
        bool retain = false,
        CancellationToken ct = default);

    Task Publish(
        string topic,
        CancellationToken ct = default);
}
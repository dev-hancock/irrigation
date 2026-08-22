namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMqttConsumer
{
    ValueTask Consume(Message message, CancellationToken ct = default);
}
using ErrorOr;

namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMqttConsumer
{
    Task<ErrorOr<Success>> Consume(Message message, CancellationToken ct = default);
}
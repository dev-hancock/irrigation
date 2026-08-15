using ErrorOr;
using Irrigation.Application.Extensions;
using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Mqtt;

public class MqttConsumer(IEnumerable<IMessageHandler> handlers) : IMqttConsumer
{
    public async Task<ErrorOr<Success>> Consume(Message message, CancellationToken ct = default)
    {
        foreach (var handler in handlers)
        {
            ct.ThrowIfCancellationRequested();

            if (!handler.CanHandle(message))
            {
                continue;
            }

            var result = await handler.Handle(message, ct);

            result.ThrowIfError();

            return result;
        }

        return Error.NotFound("Mqtt.Handler.NotFound", $"No handler registered for MQTT topic: {message.Topic}");
    }
}
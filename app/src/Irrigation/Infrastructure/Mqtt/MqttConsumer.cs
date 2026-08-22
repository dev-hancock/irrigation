using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Mqtt;

public sealed partial class MqttConsumer(IEnumerable<IMessageHandler> handlers, ILogger<MqttConsumer> _) : IMqttConsumer
{
    public async ValueTask Consume(Message message, CancellationToken ct = default)
    {
        foreach (var handler in handlers)
        {
            ct.ThrowIfCancellationRequested();

            if (!handler.CanHandle(message))
            {
                continue;
            }

            try
            {
                await handler.Handle(message, ct);
            }
            catch (Exception ex)
            {
                LogHandlerFailed(ex, message.Topic.Value, handler.GetType().Name);
            }

            return;
        }

        LogHandlerNotFound(message.Topic.Value);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to handle MQTT message '{Topic}' with '{Handler}'.")]
    partial void LogHandlerFailed(Exception exception,  string topic, string handler);

    [LoggerMessage(Level = LogLevel.Warning, Message = "No handler registered for MQTT topic '{Topic}'.")]
    partial void LogHandlerNotFound(string topic);
}
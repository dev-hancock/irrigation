namespace Irrigation.Infrastructure.Mqtt
{
    public interface IMqttClient
    {
        Task Publish(string topic, object payload, bool retain = false, CancellationToken ct = default);

        Task Publish(string topic, CancellationToken ct = default);
    }

    public class MqttConsumer
    {

    }

    public interface IMessageHandler
    {
        bool CanHandle(Message message);

        Task Handle(Message message, CancellationToken ct);
    }
}

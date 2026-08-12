using ErrorOr;
using Irrigation.Application.Extensions;

namespace Irrigation.Infrastructure.Mqtt;

public interface IMqttClient
{
    Task Publish(string topic, object payload, bool retain = false, CancellationToken ct = default);

    Task Publish(string topic, CancellationToken ct = default);

    Task Start(CancellationToken ct = default);
}

public class MqttClient : IMqttClient
{
    public Task Publish(string topic, object payload, bool retain = false, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task Publish(string topic, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task Start(CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}

public interface IMqttConsumer
{
    Task<ErrorOr<Success>> Consume(Message message, CancellationToken ct = default);
}

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

public interface IMessageHandler
{
    bool CanHandle(Message message);

    Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct);
}

public class MqttService(IMqttClient client) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await client.Start(stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
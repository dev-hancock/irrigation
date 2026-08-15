using System.Text.Json;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using MQTTnet;

namespace Irrigation.Infrastructure.Mqtt;

public class MqttPublisher : IMqttPublisher
{
    private readonly IMqttClient _client;

    public MqttPublisher(IMqttClient client)
    {
        _client = client;
    }

    public async Task Publish(string topic, object payload, bool retain = false, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(payload);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(retain)
            .Build();

        await _client.PublishAsync(message, ct);
    }

    public async Task Publish(string topic, CancellationToken ct = default)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .Build();

        await _client.PublishAsync(message, ct);
    }
}
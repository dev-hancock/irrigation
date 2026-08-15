using System.Text;
using ErrorOr;
using Irrigation.Application.Extensions;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace Irrigation.Infrastructure.Mqtt;

public class MqttConnection : IMqttConnection
{
    private readonly IMqttClient _client;

    private readonly IMqttConsumer _consumer;

    private readonly MqttOptions _options;

    public MqttConnection(IMqttClient client, IMqttConsumer consumer, IOptions<MqttOptions> options)
    {
        _client = client;
        _options = options.Value;
        _consumer = consumer;

        _client.ApplicationMessageReceivedAsync += OnMessageReceived;
    }

    public bool IsConnected => _client.IsConnected;

    public async Task<ErrorOr<Success>> Connect(CancellationToken ct = default)
    {
        var connect = new MqttClientOptionsBuilder()
            .WithClientId(_options.ClientId)
            .WithTcpServer(_options.Host, _options.Port)
            .WithCredentials(
                _options.Username,
                _options.Password)
            .Build();

        var result = await _client.ConnectAsync(connect, ct);

        if (!IsSuccess(result.ResultCode))
        {
            return Error.Failure(
                "Mqtt.Connection.Failure",
                $"Failed to connect to MQTT broker: {result.ReasonString}");
        }

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> Subscribe(string pattern, CancellationToken ct = default)
    {
        var subscription = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(pattern)
            .Build();

        var result = await _client.SubscribeAsync(subscription, ct);

        var failed = result.Items
            .Where(x => !IsSuccess(x.ResultCode))
            .Select(x => Error.Failure(
                "Mqtt.Subscription.Failure",
                $"Failed to subscribe to topic: {x.TopicFilter.Topic}"))
            .ToArray();

        if (failed.Any())
        {
            return failed;
        }

        return Result.Success;
    }

    private static bool IsSuccess(MqttClientConnectResultCode code)
    {
        return code is MqttClientConnectResultCode.Success;
    }

    private static bool IsSuccess(MqttClientSubscribeResultCode code)
    {
        return code is
            MqttClientSubscribeResultCode.GrantedQoS0 or
            MqttClientSubscribeResultCode.GrantedQoS1 or
            MqttClientSubscribeResultCode.GrantedQoS2;
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
    {
        var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);

        var message = new Message
        {
            Topic = new Topic(arg.ApplicationMessage.Topic), Payload = payload
        };

        var result = await _consumer.Consume(message);

        result.ThrowIfError();
    }
}
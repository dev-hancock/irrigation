using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Mqtt;

public sealed partial class MqttService(IServiceScopeFactory factory, ILogger<MqttService> _) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = factory.CreateScope();

        var connection = scope.ServiceProvider.GetRequiredService<IMqttConnection>();

        while (!stoppingToken.IsCancellationRequested)
        {
            if (!connection.IsConnected)
            {
                var connect = await connection.Connect(
                    stoppingToken);

                if (connect.IsError)
                {
                    LogConnectionFailed(connect.Errors);

                    await Delay(stoppingToken);
                    continue;
                }

                var subscribe = await connection.Subscribe(
                    "irrigation/+/event/#",
                    stoppingToken);

                if (subscribe.IsError)
                {
                    LogSubscriptionFailed(subscribe.Errors);
                }
            }

            await Delay(stoppingToken);
        }
    }

    private static Task Delay(CancellationToken ct)
    {
        return Task.Delay(TimeSpan.FromSeconds(5), ct);
    }

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Unable to connect to MQTT broker: {Errors}")]
    partial void LogConnectionFailed(IEnumerable<ErrorOr.Error> errors);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Unable to subscribe to MQTT topics: {Errors}")]
    partial void LogSubscriptionFailed(IEnumerable<ErrorOr.Error> errors);
}
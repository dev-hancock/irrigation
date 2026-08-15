using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Mqtt;

public class MqttService(IServiceScopeFactory factory, ILogger<MqttService> logger) : BackgroundService
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
                    logger.LogWarning(
                        "Unable to connect to MQTT broker: {Errors}",
                        connect.Errors);

                    await Delay(stoppingToken);
                    continue;
                }

                var subscribe = await connection.Subscribe(
                    "irrigation/+/event/#",
                    stoppingToken);

                if (subscribe.IsError)
                {
                    logger.LogWarning(
                        "Unable to subscribe to MQTT topics: {Errors}",
                        subscribe.Errors);
                }
            }

            await Delay(stoppingToken);
        }
    }

    private static Task Delay(CancellationToken ct)
    {
        return Task.Delay(TimeSpan.FromSeconds(5), ct);
    }
}
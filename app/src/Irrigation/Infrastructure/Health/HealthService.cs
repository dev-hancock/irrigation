using ErrorOr;
using Irrigation.Application.Health;
using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Health;

public class HealthService(IMqttPublisher client) : IHealthService
{
    public async Task<ErrorOr<Success>> Heartbeat(CancellationToken ct = default)
    {
        var topic = "irrigation/ping";

        await client.Publish(topic, ct);

        return Result.Success;
    }
}
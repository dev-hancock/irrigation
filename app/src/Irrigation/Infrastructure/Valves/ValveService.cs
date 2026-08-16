using ErrorOr;
using Irrigation.Application.Valves;
using Irrigation.Domain.Shared;
using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Valves;

public class ValveService(IMqttPublisher client) : IValveService
{
    public async Task<ErrorOr<Success>> Open(int index, HardwareId device, CancellationToken ct = default)
    {
        var message = new
        {
            id = index
        };

        var topic = $"irrigation/{device.Value}/command/valve/open";

        await client.Publish(topic, message, false, ct);

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> Close(int index, HardwareId device, CancellationToken ct = default)
    {
        var message = new
        {
            id = index
        };

        var topic = $"irrigation/{device.Value}/command/valve/close";

        await client.Publish(topic, message, false, ct);

        return Result.Success;
    }
}
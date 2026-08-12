using ErrorOr;
using Irrigation.Application.Valves;
using Irrigation.Domain.Valves;
using Irrigation.Infrastructure.Mqtt;

namespace Irrigation.Infrastructure.Ports;

public class ValveService(IMqttClient client) : IValveService
{
    public async Task<ErrorOr<Success>> Open(ValveId id, CancellationToken ct = default)
    {
        var message = new ValveMessage
        {
            Id = id.ToString()
        };

        await client.Publish(ValveTopics.Open, message, false, ct);

        return Result.Success;
    }

    public async Task<ErrorOr<Success>> Close(ValveId id, CancellationToken ct = default)
    {
        var message = new ValveMessage
        {
            Id = id.ToString()
        };

        await client.Publish(ValveTopics.Close, message, false, ct);

        return Result.Success;
    }
}
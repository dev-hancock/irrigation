using System.Text.Json;
using System.Text.RegularExpressions;
using ErrorOr;
using Irrigation.Application.Devices.Events;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Health;

public sealed partial class HealthMessageHandler(IMediator mediator) : IMessageHandler
{
    [GeneratedRegex("/pong$")]
    private static partial Regex Pattern();

    public bool CanHandle(Message message)
    {
        return Pattern().IsMatch(message.Topic.Value);
    }

    public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<HeartbeatMessage>(message.Payload);

        if (payload is null)
        {
            return Result.Success;
        }

        return await mediator.Send(
            new DeviceHeartbeatCommand
            {
                Id = message.Device
            }, ct);
    }
}
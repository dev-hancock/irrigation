using System.Text.Json;
using System.Text.RegularExpressions;
using Irrigation.Application.Health.Events;
using Irrigation.Domain.Shared;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Health;

public sealed partial class HealthMessageHandler(IMediator mediator) : IMessageHandler
{
    public bool CanHandle(Message message)
    {
        return Pattern().IsMatch(message.Topic.Value);
    }

    public async ValueTask Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<HeartbeatMessage>(message.Payload);

        if (payload is null)
        {
            return;
        }

        await mediator.Publish(
            new HeartbeatReceivedEvent
            {
                Id = HardwareId.From(message.Device)
            }, ct);
    }

    [GeneratedRegex("/pong$")]
    private static partial Regex Pattern();
}
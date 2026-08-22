using System.Text.Json;
using System.Text.RegularExpressions;
using Irrigation.Application.Valves.Events.Inbound.UpdateValve;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Valves;

public sealed partial class ValveMessageHandler(IMediator mediator) : IMessageHandler
{
    public bool CanHandle(Message message)
    {
        return Pattern().IsMatch(message.Topic.Value);
    }

    public async ValueTask Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<ValveMessage>(message.Payload);

        if (payload is null)
        {
            return;
        }

        if (!Enum.TryParse<ValveStatus>(payload.Status, true, out var status) || !Enum.IsDefined(status))
        {
            return;
        }

        await mediator.Publish(
            new UpdateValveEvent
            {
                Index = payload.Index, 
                Device = HardwareId.From(message.Device), 
                Status = status
            }, ct);
    }

    [GeneratedRegex(@"/event/valve/\d+/state$")]
    private static partial Regex Pattern();
}
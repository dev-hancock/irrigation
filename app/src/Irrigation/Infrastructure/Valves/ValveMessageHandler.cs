using System.Text.Json;
using System.Text.RegularExpressions;
using ErrorOr;
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

    public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<ValveMessage>(message.Payload);

        if (payload is null)
        {
            return Result.Success;
        }

        if (!Enum.TryParse<ValveStatus>(payload.Status, true, out var status) || !Enum.IsDefined(status))
        {
            return Error.Validation("Valve.InvalidStatus", $"Invalid valve status '{payload.Status}'.");
        }

        await mediator.Publish(
            new UpdateValveEvent
            {
                Index = payload.Id, Device = HardwareId.From(message.Device), Status = status
            }, ct);

        return Result.Success;
    }

    [GeneratedRegex(@"/event/valve/\d+/state$")]
    private static partial Regex Pattern();
}
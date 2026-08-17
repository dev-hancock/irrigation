using ErrorOr;
using Irrigation.Application.Valves.Commands;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;
using System.Text.Json;
using System.Text.RegularExpressions;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Infrastructure.Valves;

public sealed partial class ValveMessageHandler(IMediator mediator) : IMessageHandler
{
    [GeneratedRegex(@"/event/valve/\d+/state$")]
    private static partial Regex Pattern();

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

        var status = Enum.Parse<ValveStatus>(payload.Status, true);

        return await mediator.Send(
            new UpdateValveCommand
            {
                Index = payload.Id, 
                Device = HardwareId.From(message.Device), 
                Status = status
            }, ct);
    }
}
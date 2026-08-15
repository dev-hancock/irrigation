using System.Text.Json;
using ErrorOr;
using Irrigation.Application.Valves.Commands;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Ports.Valves;

public sealed class ValveMessageHandler(IMediator mediator) : IMessageHandler
{
    public bool CanHandle(Message message)
    {
        // irrigation/{device}/event/valve/{id}/state
        return message.Topic.Length == 6
               && message[2] == "event"
               && message[3] == "valve"
               && message[5] == "state";
    }

    public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<ValveMessage>(message.Payload);

        if (payload is null)
        {
            return Result.Success;
        }

        var id = message.Topic[4];

        return await mediator.Send(
            new UpdateValveCommand
            {
                Id = id, 
                Device = message.Device, 
                Status = payload.Status
            }, ct);
    }
}
using Irrigation.Application.Valves.Commands;
using Irrigation.Infrastructure.Mqtt;
using System.Text.Json;
using ErrorOr;
using Mediator;

namespace Irrigation.Infrastructure.Ports
{
    public sealed class ValveMessageHandler(IMediator mediator) : IMessageHandler
    {
        public bool CanHandle(Message message)
        {
            return message.Route.StartsWith("valve/");
        }

        public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
        {
            var payload = JsonSerializer.Deserialize<ValveMessage>(message.Payload);

            if (payload is null)
            {
                return Result.Success;
            }

            if (message.Route == ValveTopics.Opened)
            {
                return await mediator.Send(
                    new ValveOpenedCommand
                {
                    Id = payload.Id,
                    Device = message.Device
                }, ct);
            }

            if (message.Route == ValveTopics.Closed)
            {
                return await mediator.Send(
                    new ValveClosedCommand
                {
                    Id = payload.Id,
                    Device = message.Device
                }, ct);
            }

            return Result.Success;
        }
    }
}

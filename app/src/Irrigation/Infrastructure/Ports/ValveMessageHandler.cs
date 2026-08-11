using Irrigation.Application.Common;
using Irrigation.Application.Valves.Commands;
using Irrigation.Infrastructure.Mqtt;
using System.Text.Json;

namespace Irrigation.Infrastructure.Ports
{
    public sealed class ValveMessageHandler(IMediator mediator) : IMessageHandler
    {
        public bool CanHandle(Message message)
        {
            return message.Route.StartsWith("valve/");
        }

        public Task Handle(Message message, CancellationToken ct)
        {
            var payload = JsonSerializer.Deserialize<ValveMessage>(message.Payload);

            if (payload is null)
            {
                return Task.CompletedTask; // ignore invalid messages
            }

            if (message.Route == ValveTopics.Opened)
            {
                return mediator.Send(
                    new ValveOpenedCommand
                {
                    Id = payload.Id,
                    Device = message.Device
                }, ct);
            }

            if (message.Route == ValveTopics.Closed)
            {
                return mediator.Send(
                    new ValveClosedCommand
                {
                    Id = payload.Id,
                    Device = message.Device
                }, ct);
            }

            return Task.CompletedTask;
        }
    }
}

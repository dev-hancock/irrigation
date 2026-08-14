using ErrorOr;
using Irrigation.Application.Valves.Commands;
using Irrigation.Infrastructure.Mqtt;
using Irrigation.Infrastructure.Ports.Valves;
using Mediator;
using System.Text.Json;

namespace Irrigation.Infrastructure.Ports.Devices
{
    public sealed class DeviceMessage
    {
        public string Id { get; set; }

        public string Firmware { get; set; }

        public string Model { get; set; }

        public string Version { get; set; }
    }

    public sealed class DeviceMessageHandler(IMediator mediator) : IMessageHandler
    {
        public bool CanHandle(Message message)
        {
            // irrigation/{device}/event/device
            return message.Topic.Length == 4
                   && message[2] == "event"
                   && message[3] == "device";
        }

        public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
        {
            var payload = JsonSerializer.Deserialize<DeviceMessage>(message.Payload);

            if (payload is null)
            {
                return Result.Success;
            }

            return await mediator.Send(
                new UpdateDeviceCommand
                {
                    Id = payload.Id,
                    Device = message.Device
                }, ct);
        }
    }

}

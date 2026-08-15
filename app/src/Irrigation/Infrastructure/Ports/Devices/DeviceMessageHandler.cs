using System.Text.Json;
using System.Text.Json.Serialization;
using ErrorOr;
using Irrigation.Application.Devices.Commands;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Ports.Devices;

public sealed class DeviceMessage
{
    [JsonPropertyName("firmware")]
    public string Firmware { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("version")]
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
                Id = message.Device, 
                Firmware = payload.Firmware, 
                Model = payload.Model, 
                Version = payload.Version
            }, ct);
    }
}
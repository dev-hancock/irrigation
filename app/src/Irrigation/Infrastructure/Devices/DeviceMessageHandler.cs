using System.Text.Json;
using System.Text.RegularExpressions;
using Irrigation.Application.Devices.Events.Inbound;
using Irrigation.Domain.Shared;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;

namespace Irrigation.Infrastructure.Devices;

public sealed partial class DeviceMessageHandler(IMediator mediator) : IMessageHandler
{
    public bool CanHandle(Message message)
    {
        return Pattern().IsMatch(message.Topic.Value);
    }

    public async ValueTask Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<DeviceMessage>(message.Payload);

        if (payload is null)
        {
            return;
        }

        await mediator.Publish(
            new UpdateDeviceEvent
            {
                Id = HardwareId.From(message.Device), Firmware = payload.Firmware, Model = payload.Model, Version = payload.Version
            }, ct);
    }

    [GeneratedRegex("/event/device$")]
    private static partial Regex Pattern();
}
using ErrorOr;
using Irrigation.Application.Devices.Events;
using Irrigation.Domain.Shared;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Mediator;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Irrigation.Infrastructure.Devices;

public sealed partial class DeviceMessageHandler(IMediator mediator) : IMessageHandler
{
    [GeneratedRegex("/event/device$")]
    private static partial Regex Pattern();

    public bool CanHandle(Message message)
    {
        return Pattern().IsMatch(message.Topic.Value);
    }

    public async Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct)
    {
        var payload = JsonSerializer.Deserialize<DeviceMessage>(message.Payload);

        if (payload is null)
        {
            return Result.Success;
        }

        await mediator.Publish(
            new UpdateDeviceEvent
            {
                Id = HardwareId.From(message.Device),
                Firmware = payload.Firmware, 
                Model = payload.Model, 
                Version = payload.Version
            }, ct);

        return Result.Success;
    }
}
using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Application.Devices.Events.Inbound;

public class UpdateDeviceEvent : INotification
{
    public required HardwareId Id { get; set; }

    public required string Firmware { get; set; }

    public required string Model { get; set; }

    public required string Version { get; set; }
}
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Events.Inbound.UpdateValve;

public sealed record UpdateValveEvent : INotification
{
    public required HardwareId Device { get; set; }

    public required int Index { get; set; }

    public required ValveStatus Status { get; set; }
}
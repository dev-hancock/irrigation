using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Valves;

public class Valve : AggregateRoot
{
    private Valve()
    {
        // EF Core
    }

    private Valve(ValveId id, DeviceId deviceId, HardwareId hardwareId, ValveStatus status)
    {
        Id = id;
        DeviceId = deviceId;
        HardwareId = hardwareId;
        Status = status;
    }

    public ValveId Id { get; }

    public DeviceId DeviceId { get; private set; }

    public HardwareId HardwareId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public ValveStatus Status { get; private set; }

    public static Valve Create(DeviceId deviceId, HardwareId hardwareId, ValveStatus status)
    {
        return new Valve(
            ValveId.New(),
            deviceId,
            hardwareId,
            status
        );
    }

    public void Open()
    {
        if (Status is ValveStatus.Opened or ValveStatus.Opening)
        {
            return;
        }

        Status = ValveStatus.Opening;

        Raise(new ValveOpeningEvent
        {
            Id = Id
        });
    }

    public void Rename(string name)
    {
        if (name == Name)
        {
            return;
        }

        Name = name;

        Raise(new ValveRenamedEvent
        {
            Id = Id, Name = name
        });
    }

    public void SetStatus(ValveStatus status)
    {
        if (status == Status)
        {
            return;
        }

        Status = status;

        Raise(new ValveStatusChangedEvent
        {
            Id = Id, Status = status
        });
    }

    public void Close()
    {
        if (Status is ValveStatus.Closed or ValveStatus.Closing)
        {
            return;
        }

        Status = ValveStatus.Closing;

        Raise(new ValveClosingEvent
        {
            Id = Id
        });
    }
}
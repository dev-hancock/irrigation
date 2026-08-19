using Irrigation.Domain.Activities;
using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves.Events;

namespace Irrigation.Domain.Valves;

public class Valve : AggregateRoot
{
    private Valve()
    {
        // EF Core
    }

    private Valve(ValveId id, DeviceId deviceId, int index, ValveStatus status, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Id = id;
        DeviceId = deviceId;
        Index = index;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public ValveId Id { get; }

    public DeviceId DeviceId { get; }

    public int Index { get; }

    public string Name { get; private set; } = string.Empty;

    public ValveStatus Status { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static Valve Create(DeviceId deviceId, int index, ValveStatus status)
    {
        return new Valve(
            ValveId.New(),
            deviceId,
            index,
            status,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow
        );
    }

    public void Open()
    {
        if (Status is ValveStatus.Open or ValveStatus.Opening)
        {
            return;
        }

        Status = ValveStatus.Opening;

        Raise(new ValveOpeningEvent
        {
            Id = Id, Index = Index, DeviceId = DeviceId
        });

        UpdatedAt = DateTimeOffset.UtcNow;
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

        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Fault()
    {
        if (Status == ValveStatus.Faulted)
        {
            return;
        }

        Status = ValveStatus.Faulted;
        UpdatedAt = DateTimeOffset.UtcNow;

        Raise(new ValveFaultedEvent
        {
            Id = Id,
            Name = Name,
            Index = Index,
            DeviceId = DeviceId
        });
    }

    public void SetStatus(ValveStatus status)
    {
        if (Status == status)
        {
            return;
        }

        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Opened(ActivityOrigin origin)
    {
        Raise(new ValveOpenedEvent
        {
            Id = Id, Name = Name, Index = Index, DeviceId = DeviceId, Origin = origin
        });
    }

    public void Closed(ActivityOrigin origin)
    {
        Raise(new ValveClosedEvent
        {
            Id = Id, Name = Name, Index = Index, DeviceId = DeviceId, Origin = origin
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
            Id = Id, Index = Index, DeviceId = DeviceId
        });

        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
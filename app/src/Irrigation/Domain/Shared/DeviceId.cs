namespace Irrigation.Domain.Shared;

public readonly record struct DeviceId(Guid Value)
{
    public static DeviceId New()
    {
        return new DeviceId(Guid.NewGuid());
    }

    public static DeviceId? From(Guid? id)
    {
        return id is null ? null : new DeviceId(id.Value);
    }

    public static DeviceId From(Guid id)
    {
        return new DeviceId(id);
    }

    public static implicit operator Guid(DeviceId id)
    {
        return id.Value;
    }

    public static explicit operator DeviceId(Guid value)
    {
        return new DeviceId(value);
    }
}
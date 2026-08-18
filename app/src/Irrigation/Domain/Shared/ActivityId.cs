namespace Irrigation.Domain.Shared;

public readonly record struct ActivityId(Guid Value)
{
    public static ActivityId New()
    {
        return new ActivityId(Guid.NewGuid());
    }

    public static ActivityId? From(Guid? id)
    {
        return id is null ? null : new ActivityId(id.Value);
    }

    public static ActivityId From(Guid id)
    {
        return new ActivityId(id);
    }

    public static implicit operator Guid(ActivityId id)
    {
        return id.Value;
    }

    public static explicit operator ActivityId(Guid value)
    {
        return new ActivityId(value);
    }
}
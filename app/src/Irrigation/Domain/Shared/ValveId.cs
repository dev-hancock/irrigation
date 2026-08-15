namespace Irrigation.Domain.Shared;

public readonly record struct ValveId(Guid Value)
{
    public static ValveId New()
    {
        return new ValveId(Guid.NewGuid());
    }

    public static implicit operator Guid(ValveId id)
    {
        return id.Value;
    }

    public static explicit operator ValveId(Guid value)
    {
        return new ValveId(value);
    }

    public static ValveId From(Guid id)
    {
        return new ValveId(id);
    }

    public static ValveId? From(Guid? id)
    {
        return id is null ? null : new ValveId(id.Value);
    }
}
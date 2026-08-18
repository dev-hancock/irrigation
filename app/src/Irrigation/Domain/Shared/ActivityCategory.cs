namespace Irrigation.Domain.Shared;

public readonly record struct ActivityCategory(string Value)
{
    public static ActivityCategory From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new ActivityCategory(value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(ActivityCategory type)
    {
        return type.Value;
    }

    public static explicit operator ActivityCategory(string value)
    {
        return new ActivityCategory(value);
    }
}
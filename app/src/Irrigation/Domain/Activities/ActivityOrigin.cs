namespace Irrigation.Domain.Activities;

public readonly record struct ActivityOrigin
{
    private readonly string _value;

    private ActivityOrigin(string value)
    {
        _value = value;
    }

    public static ActivityOrigin Manual => new("Manual");

    public static ActivityOrigin Schedule => new("Schedule");

    public static ActivityOrigin System => new("System");

    public static ActivityOrigin From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new ActivityOrigin(value);
    }

    public override string ToString()
    {
        return _value;
    }


    public static implicit operator string(ActivityOrigin type)
    {
        return type._value;
    }

    public static explicit operator ActivityOrigin(string value)
    {
        return new ActivityOrigin(value);
    }
}
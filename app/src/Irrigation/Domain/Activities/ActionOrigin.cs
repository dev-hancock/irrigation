namespace Irrigation.Domain.Activities;

public readonly record struct ActionOrigin
{
    private readonly string _value;

    private ActionOrigin(string value)
    {
        _value = value;
    }

    public static ActionOrigin Manual { get; } = new("Manual");

    public static ActionOrigin Schedule { get; } = new("Schedule");

    public static ActionOrigin System { get; } = new("System");

    public static ActionOrigin From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new ActionOrigin(value);
    }

    public override string ToString()
    {
        return _value;
    }


    public static implicit operator string(ActionOrigin type)
    {
        return type._value;
    }

    public static explicit operator ActionOrigin(string value)
    {
        return new ActionOrigin(value);
    }
}
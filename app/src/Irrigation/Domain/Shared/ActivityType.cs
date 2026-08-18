namespace Irrigation.Domain.Shared;

public readonly record struct ActivityType(string Value)
{
    public static ActivityType From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new ActivityType(value);
    }

    public override string ToString() => Value;
}
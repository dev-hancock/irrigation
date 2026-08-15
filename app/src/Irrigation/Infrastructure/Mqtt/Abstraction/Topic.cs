namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public sealed record Topic
{
    private readonly string[] _parts;

    public Topic(string value)
    {
        Value = value;

        _parts = value.Split('/', StringSplitOptions.RemoveEmptyEntries);
    }

    public string Value { get; }

    public string Device => _parts[1];

    public int Length => _parts.Length;

    public string this[int index] => _parts[index];

    public override string ToString()
    {
        return Value;
    }
}
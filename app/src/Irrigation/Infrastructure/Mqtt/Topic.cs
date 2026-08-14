namespace Irrigation.Infrastructure.Mqtt;

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

    public int Length => _parts.Length - 3;

    public string this[int index] => _parts[index + 3];

    public override string ToString() => Value;
}
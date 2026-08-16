namespace Irrigation.Domain.Shared;

public readonly record struct HardwareId(string Value)
{
    public static HardwareId From(string id)
    {
        return new HardwareId(id);
    }

    public static implicit operator string(HardwareId id)
    {
        return id.Value;
    }
}
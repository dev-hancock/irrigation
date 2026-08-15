using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Devices;

public class Device : AggregateRoot
{
    private Device()
    {
        // EF Core
    }

    private Device(DeviceId id, HardwareId hardwareId, string firmware, string model, string version)
    {
        Id = id;
        HardwareId = hardwareId;
        Firmware = firmware;
        Model = model;
        Version = version;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public DeviceId Id { get; private set; }

    public HardwareId HardwareId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Firmware { get; private set; } = string.Empty;

    public string Model { get; private set; } = string.Empty;

    public string Version { get; private set; } = string.Empty;

    public DateTimeOffset UpdatedAt { get; private set; }

    public static Device Create(
        HardwareId hardwareId,
        string firmware,
        string model,
        string version)
    {
        return new Device(
            DeviceId.New(),
            hardwareId,
            firmware,
            model,
            version);
    }

    public void Update(string firmware, string model, string version)
    {
        var hasChanged =
            Firmware != firmware ||
            Model != model ||
            Version != version;

        if (!hasChanged)
        {
            return;
        }

        Firmware = firmware;
        Model = model;
        Version = version;

        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string name)
    {
        if (name == Name)
        {
            return;
        }

        Name = name;
    }
}
namespace Irrigation.Domain.Devices
{
    public readonly record struct DeviceId(Guid Value);

    public readonly record struct HardwareId(string Value);

    public class Device
    {
        public DeviceId Id { get; set; }

        public HardwareId HardwareId { get; set; }

        public string Name { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Devices;

public sealed class DeviceMessage
{
    [JsonPropertyName("firmware")]
    public string Firmware { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }
}
using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Health;

public sealed class HeartbeatMessage
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}
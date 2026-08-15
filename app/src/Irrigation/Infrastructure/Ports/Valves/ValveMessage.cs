using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Ports.Valves;

public sealed record ValveMessage
{
    [JsonPropertyName("status")]
    public required string Status { get; init; }
}
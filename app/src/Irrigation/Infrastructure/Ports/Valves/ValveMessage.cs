using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Ports.Valves;

public sealed record ValveMessage
{
    [JsonPropertyName("state")]
    public required string State { get; init; }
}
using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Valves;

public sealed record ValveMessage
{
    [JsonPropertyName("id")]
    public required int Index { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }
}
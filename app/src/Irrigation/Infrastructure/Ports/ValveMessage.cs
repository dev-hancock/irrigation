using System.Text.Json.Serialization;

namespace Irrigation.Infrastructure.Ports
{
    public sealed record ValveMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; init; }
    }
}

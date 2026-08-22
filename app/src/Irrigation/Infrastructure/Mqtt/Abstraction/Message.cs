using Irrigation.Domain.Shared;

namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public sealed record Message : IIdentified
{
    public Guid EventId { get; init; }

    public string Device => Topic.Device;

    public required Topic Topic { get; init; }

    public required string Payload { get; init; }

    public string this[int index] => Topic[index];
}
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves;

public sealed record ValveChanged
{
    public ValveId Id { get; init; }
}
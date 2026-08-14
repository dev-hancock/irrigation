using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves;

public sealed record ValveStateChanged
{
    public ValveId Id { get; init; }

    public ValveStatus Status { get; init; }
}
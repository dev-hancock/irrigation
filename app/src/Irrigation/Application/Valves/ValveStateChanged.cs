using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves
{
    public sealed record ValveStateChanged
    {
        public ValveId Id { get; init; }

        public ValveState State { get; init; }
    }
}

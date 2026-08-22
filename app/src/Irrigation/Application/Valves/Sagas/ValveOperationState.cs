using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves.Sagas;

public class ValveOperationState
{
    public required ValveStatus Target { get; init; }

    public required ValveId ValveId { get; init; }

    public required ActionOrigin Origin { get; init; }
}
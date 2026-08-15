using Ardalis.Specification;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Domain.Specifications;

public class GetValveSpec : Specification<Valve>
{
    public GetValveSpec(ValveId id)
    {
        // todo
    }

    public GetValveSpec(HardwareId id)
    {
    }
}
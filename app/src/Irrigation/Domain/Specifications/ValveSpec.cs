using Ardalis.Specification;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Domain.Specifications;

public class ValveSpec : Specification<Valve>
{
    public ValveSpec(ValveId id)
    {
        Query.Where(x => x.Id == id);
    }

    public ValveSpec(int index, DeviceId device)
    {
        Query.Where(x => x.Index == index && x.DeviceId == device);
    }
}
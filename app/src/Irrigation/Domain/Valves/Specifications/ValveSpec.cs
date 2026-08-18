using Ardalis.Specification;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Valves.Specifications;

public class ValveSpec : Specification<Valve>
{
    public ValveSpec(ValveId id)
    {
        Query.Where(x => x.Id == id);
    }

    public ValveSpec(DeviceId device, int index)
    {
        Query.Where(x => x.Index == index && x.DeviceId == device);
    }
}
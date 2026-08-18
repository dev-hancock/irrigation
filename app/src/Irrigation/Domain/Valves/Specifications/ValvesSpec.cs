using Ardalis.Specification;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Valves.Specifications;

public class ValvesSpec : Specification<Valve>
{
    public ValvesSpec(DeviceId? id)
    {
        if (id.HasValue)
        {
            Query.Where(x => x.DeviceId == id.Value);
        }
    }
}
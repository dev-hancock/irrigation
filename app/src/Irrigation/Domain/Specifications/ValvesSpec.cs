using Ardalis.Specification;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Domain.Specifications;

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

public class ValvesReadOnlySpec : Specification<Valve>
{
    public ValvesReadOnlySpec(DeviceId? id)
    {
        Query.AsNoTracking();

        if (id is not null)
        {
            Query.Where(x => x.DeviceId == id.Value);
        }
    }
}
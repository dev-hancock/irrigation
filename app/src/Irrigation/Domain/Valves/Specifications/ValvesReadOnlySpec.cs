using Ardalis.Specification;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Valves.Specifications;

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
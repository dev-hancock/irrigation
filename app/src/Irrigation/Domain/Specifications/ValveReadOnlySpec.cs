using Ardalis.Specification;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;

namespace Irrigation.Domain.Specifications;

public class ValveReadOnlySpec : Specification<Valve>
{
    public ValveReadOnlySpec(ValveId id)
    {
        Query.AsNoTracking().Where(x => x.Id == id);
    }
}
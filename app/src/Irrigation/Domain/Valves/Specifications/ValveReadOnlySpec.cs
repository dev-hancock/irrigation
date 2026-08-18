using Ardalis.Specification;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Valves.Specifications;

public class ValveReadOnlySpec : Specification<Valve>
{
    public ValveReadOnlySpec(ValveId id)
    {
        Query.AsNoTracking().Where(x => x.Id == id);
    }
}
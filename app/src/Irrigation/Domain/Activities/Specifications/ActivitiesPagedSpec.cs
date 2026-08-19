using Ardalis.Specification;

namespace Irrigation.Domain.Activities.Specifications;

public class ActivitiesPagedSpec : Specification<Activity>
{
    public ActivitiesPagedSpec(int page, int count)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(x => x.Timestamp)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * count)
            .Take(count);
    }
}
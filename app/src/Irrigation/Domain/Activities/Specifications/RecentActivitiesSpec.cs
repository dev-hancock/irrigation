using Ardalis.Specification;

namespace Irrigation.Domain.Activities.Specifications;

public class RecentActivitiesSpec : Specification<Activity>
{
    public RecentActivitiesSpec(int count)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(x => x.Timestamp)
            .Take(count);
    }
}
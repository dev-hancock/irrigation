using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Activities;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetRecentActivities;

public class GetRecentActivitiesHandler(IRepository<Activity> repo, IActivityMapper mapper)
    : IRequestHandler<GetRecentActivitiesQuery, ErrorOr<ActivityModel[]>>
{
    public async ValueTask<ErrorOr<ActivityModel[]>> Handle(GetRecentActivitiesQuery request, CancellationToken cancellationToken)
    {
        var activities = await repo.ListAsync(
            new RecentActivitiesSpec(request.Count),
            cancellationToken);

        return activities
            .Select(mapper.Map)
            .ToList();
    }
}
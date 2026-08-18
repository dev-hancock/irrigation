using System.Diagnostics;
using ErrorOr;
using Irrigation.Application.Common;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetPagedActivities;

public class GetPagedActivitiesHandler(IRepository<Activity> repo, IActivityMapper mapper)
    : IRequestHandler<GetPagedActivitiesQuery, ErrorOr<ActivityModel[]>>
{
    public async ValueTask<ErrorOr<ActivityModel[]>> Handle(GetPagedActivitiesQuery request, CancellationToken cancellationToken)
    {
        var page = await repo.ListAsync(
            new ActivitiesPagedSpec(
                request.Page,
                request.Count),
            cancellationToken);

        var count = await repo.CountAsync(
            new ActivitiesSpec(),
            cancellationToken);

        return new PagedResult<ActivityDto>(
            page.Select(mapper.Map).ToList(),
            count,
            request.Page,
            request.Count);
    }
}
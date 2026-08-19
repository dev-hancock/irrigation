using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Application.Common.Pagination;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Activities.Specifications;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetPagedActivities;

public class GetPagedActivitiesHandler(IRepository<Activity> repo, IActivityMapper mapper)
    : IRequestHandler<GetPagedActivitiesQuery, ErrorOr<PagedResult<ActivityModel>>>
{
    public async ValueTask<ErrorOr<PagedResult<ActivityModel>>> Handle(GetPagedActivitiesQuery request, CancellationToken cancellationToken)
    {
        var page = await repo.ListAsync(
            new ActivitiesPagedSpec(
                request.Page,
                request.Count),
            cancellationToken);

        var count = await repo.CountAsync(
            new ActivitiesSpec(),
            cancellationToken);

        return new PagedResult<ActivityModel>
        {
            Items = page.Select(mapper.Map).ToList(), TotalCount = count, Page = request.Page, PageSize = request.Count
        };
    }
}
using ErrorOr;
using Irrigation.Application.Common.Pagination;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetPagedActivities;

public sealed record GetPagedActivitiesQuery : IRequest<ErrorOr<PagedResult<ActivityModel>>>
{
    public required int Page { get; init; }

    public required int Count { get; init; }
}
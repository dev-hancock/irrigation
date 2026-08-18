using ErrorOr;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetPagedActivities;

public sealed record GetPagedActivitiesQuery : IRequest<ErrorOr<ActivityModel[]>>
{
    public required int Page { get; init; }

    public required int Count { get; init; }
}
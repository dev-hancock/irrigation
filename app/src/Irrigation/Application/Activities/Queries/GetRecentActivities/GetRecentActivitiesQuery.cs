using ErrorOr;
using Mediator;

namespace Irrigation.Application.Activities.Queries.GetRecentActivities;

public sealed record GetRecentActivitiesQuery : IRequest<ErrorOr<ActivityModel[]>>
{
    public required int Count { get; init; }
}
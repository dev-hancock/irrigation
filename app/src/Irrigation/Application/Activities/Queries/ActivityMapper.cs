using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Activities;

namespace Irrigation.Application.Activities.Queries;

public class ActivityMapper(IEnumerable<IActivityFormatter> formatters) : IActivityMapper
{
    public ActivityModel Map(Activity activity)
    {
        var formatter = formatters.Single(x => x.Type == activity.Type);

        return new ActivityModel
        {
            Type = activity.Type,
            Category = activity.Category,
            Origin = activity.Origin,
            Arguments = formatter.GetArguments(activity.Data),
            Timestamp = activity.Timestamp
        };
    }
}
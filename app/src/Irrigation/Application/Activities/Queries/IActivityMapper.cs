using Irrigation.Domain.Activities;

namespace Irrigation.Application.Activities.Queries;

public interface IActivityMapper
{
    ActivityModel Map(Activity activity);
}
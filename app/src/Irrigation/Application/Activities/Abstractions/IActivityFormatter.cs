using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities.Abstractions;

public interface IActivityFormatter
{
    ActivityType Type { get; }

    string[] GetArguments(Activity activity);
}
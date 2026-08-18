using System.Text.Json;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves.Activities;

public class ValveOpenedActivityFormatter : IActivityFormatter
{
    public ActivityType Type => ValveActivity.Opened;

    public string[] GetArguments(Activity activity)
    {
        var data = JsonSerializer.Deserialize<ValveActivityData>(activity.Data!);

        if (data is null)
        {
            throw new InvalidOperationException($"Invalid activity data: {activity.Type}");
        }

        return [data.Name];
    }
}
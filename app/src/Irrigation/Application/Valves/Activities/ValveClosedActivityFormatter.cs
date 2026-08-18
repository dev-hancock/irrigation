using System.Text.Json;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves.Activities;

public class ValveClosedActivityFormatter : IActivityFormatter
{
    public ActivityType Type => ValveActivity.Closed;

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
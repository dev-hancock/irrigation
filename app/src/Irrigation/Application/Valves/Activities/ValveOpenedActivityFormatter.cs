using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Shared;
using System.Text.Json;

namespace Irrigation.Application.Valves.Activities;

public class ValveOpenedActivityFormatter : IActivityFormatter
{
    public ActivityType Type => ValveActivity.Opened;

    public string[] GetArguments(string data)
    {
        var payload = JsonSerializer.Deserialize<ValveActivityData>(data);

        if (payload is null)
        {
            throw new InvalidOperationException($"Invalid data for activity '{Type}'.");
        }

        return [payload.Name];
    }
}
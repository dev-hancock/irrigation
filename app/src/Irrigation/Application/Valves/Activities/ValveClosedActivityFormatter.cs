using System.Text.Json;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves.Activities;

public class ValveClosedActivityFormatter : IActivityFormatter
{
    public ActivityType Type => ValveActivity.Closed;

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
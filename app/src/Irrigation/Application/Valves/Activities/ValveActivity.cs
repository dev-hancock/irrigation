using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves.Activities;

public class ValveActivity
{
    public static ActivityCategory Category = ActivityCategory.From("valve");

    public static ActivityType Opened = ActivityType.From("activity.valve.opened");

    public static ActivityType Closed = ActivityType.From("activity.valve.closed");
}
using Irrigation.Domain.Activities.Events;
using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Activities;

public class Activity : AggregateRoot
{
    public ActivityId Id { get; private set; }

    public DateTimeOffset Timestamp { get; private set; }

    public ActivityType Type { get; private set; }

    public ActivityOrigin Origin { get; private set; }

    public ActivityCategory Category { get; private set; }

    public ActivitySubject? Subject { get; private set; }

    public string Data { get; private set; }

    public static Activity Create(
        ActivityType type,
        ActivityCategory category,
        ActivityOrigin origin,
        string data)
    {
        var activity = new Activity
        {
            Data = data
        };

        activity.Raise(new ActivityCreatedEvent());

        return activity;
    }
}
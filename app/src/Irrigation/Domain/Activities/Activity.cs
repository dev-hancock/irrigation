using System.Diagnostics;
using Irrigation.Domain.Common;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Activities
{
    public class Activity : AggregateRoot
    {
        public ActivityId Id { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public ActivityType Type { get; private set; }

        public ActivitySource Source { get; private set; }

        public ActivitySubject? Subject { get; private set; }

        public string Message { get; private set; }

        public Activity()
        {
            
        }

        public static Activity Create(
            ActivityType type,
            Guid subjectId,
            string message)
        {
            var activity = new Activity();

            activity.Raise(new ActivityCreatedEvent());

            return activity;
        }
    }
}

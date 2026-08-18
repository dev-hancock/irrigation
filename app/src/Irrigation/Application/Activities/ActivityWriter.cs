using ErrorOr;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities
{
    public class ActivityWriter(IRepository<Activity> repo) : IActivityWriter
    {
        public async Task<ErrorOr<Success>> Write(ActivityType type, Guid subjectId, string message, CancellationToken ct = default)
        {
            var activity = Activity.Create(
                type,
                subjectId,
                message);

            await repo.AddAsync(activity, ct);
            await repo.SaveChangesAsync(ct);

            return Result.Success;
        }
    }
}

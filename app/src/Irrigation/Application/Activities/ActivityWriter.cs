using System.Text.Json;
using ErrorOr;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Application.Common;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities;

public class ActivityWriter(IRepository<Activity> repo) : IActivityWriter
{
    public async Task<ErrorOr<Success>> Write(
        ActivityType type,
        ActivityCategory category,
        ActivityOrigin origin,
        object? data = null,
        CancellationToken ct = default)
    {
        var activity = Activity.Create(
            type,
            category,
            origin,
            JsonSerializer.Serialize(data));

        await repo.AddAsync(activity, ct);

        await repo.SaveChangesAsync(ct);

        return Result.Success;
    }
}
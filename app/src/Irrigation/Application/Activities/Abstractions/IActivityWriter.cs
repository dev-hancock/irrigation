using ErrorOr;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities.Abstractions;

public interface IActivityWriter
{
    Task<ErrorOr<Success>> Write(
        ActivityType type,
        ActivityCategory category,
        ActivityOrigin origin,
        object? data = null,
        CancellationToken ct = default);
}
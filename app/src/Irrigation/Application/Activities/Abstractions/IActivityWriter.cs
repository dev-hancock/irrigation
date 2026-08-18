using ErrorOr;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities.Abstractions
{
    public interface IActivityWriter
    {
        Task<ErrorOr<Success>> Write(
            ActivityType type,
            Guid subjectId,
            string message,
            CancellationToken ct = default);
    }
}

using ErrorOr;

namespace Irrigation.Application.Health;

public interface IHealthService
{
    Task<ErrorOr<Success>> Heartbeat(CancellationToken ct = default);
}
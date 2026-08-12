using ErrorOr;

namespace Irrigation.Domain.Valves;

public interface IValveService
{
    public Task<ErrorOr<Success>> Open(ValveId id, CancellationToken ct = default);

    public Task<ErrorOr<Success>> Close(ValveId id, CancellationToken ct = default);
}
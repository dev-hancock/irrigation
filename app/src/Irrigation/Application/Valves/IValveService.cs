using ErrorOr;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves;

public interface IValveService
{
    public Task<ErrorOr<Success>> Open(ValveId id, CancellationToken ct = default);

    public Task<ErrorOr<Success>> Close(ValveId id, CancellationToken ct = default);
}
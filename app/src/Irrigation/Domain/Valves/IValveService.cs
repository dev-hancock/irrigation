using ErrorOr;

namespace Irrigation.Domain.Valves
{
    public interface IValveService
    {
        public Task<ErrorOr<Success>> Open(Valve valve, CancellationToken ct = default);

        public Task<ErrorOr<Success>> Close(Valve valve, CancellationToken ct = default);
    }
}

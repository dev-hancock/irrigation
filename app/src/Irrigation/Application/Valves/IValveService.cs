using ErrorOr;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves;

public interface IValveService
{
    public Task<ErrorOr<Success>> Open(int index, HardwareId device, CancellationToken ct = default);

    public Task<ErrorOr<Success>> Close(int index, HardwareId device, CancellationToken ct = default);
}
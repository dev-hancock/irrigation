using ErrorOr;
using Irrigation.Domain.Shared;

namespace Irrigation.Application.Valves;

public interface IValveController
{
    public Task<ErrorOr<Success>> Open(int index, HardwareId device, CancellationToken ct = default);

    public Task<ErrorOr<Success>> Close(int index, HardwareId device, CancellationToken ct = default);
}
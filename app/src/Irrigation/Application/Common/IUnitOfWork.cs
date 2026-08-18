using Irrigation.Domain.Devices;
using Irrigation.Domain.Valves;

namespace Irrigation.Application.Common;

public interface IUnitOfWork
{
    IRepository<Device> Devices { get; }

    IRepository<Valve> Valves { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
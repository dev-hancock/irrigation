using Irrigation.Domain.Devices;
using Irrigation.Domain.Valves;

namespace Irrigation.Domain.Repository;

public interface IUnitOfWork
{
    IRepository<Device> Devices { get; }

    IRepository<Valve> Valves { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
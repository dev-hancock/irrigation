using Irrigation.Application.Common;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Valves;

namespace Irrigation.Infrastructure.Persistence;

public class UnitOfWork(IrrigationDbContext db) : IUnitOfWork
{
    public IRepository<Device> Devices { get; } = new Repository<Device>(db);

    public IRepository<Valve> Valves { get; } = new Repository<Valve>(db);

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return db.SaveChangesAsync(ct);
    }
}
using System.Text.Json;
using Irrigation.Application.Common.Sagas;
using Irrigation.Infrastructure.Persistence;

namespace Irrigation.Infrastructure.Sagas;

public sealed class SagaStore(IrrigationDbContext db) : ISagaStore
{
    public async Task<Guid> Start<TState>(TState state, CancellationToken ct = default)
    {
        var saga = new SagaInstance
        {
            Type = typeof(TState).AssemblyQualifiedName!, Data = JsonSerializer.Serialize(state)
        };

        db.Sagas.Add(saga);

        await db.SaveChangesAsync(ct);

        return saga.Id;
    }
}
using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Idempotency;

public interface IIdempotencyStore
{
    Task<bool> IsProcessed(Guid id, string handler, CancellationToken ct);

    Task Complete(Guid id, string handler, CancellationToken ct);
}

public sealed class IdempotencyStore(IrrigationDbContext db) : IIdempotencyStore
{
    public Task<bool> IsProcessed(Guid id, string handler, CancellationToken ct)
    {
        return db.Idempotency.AnyAsync(
            x =>
                x.MessageId == id &&
                x.Handler == handler,
            ct);
    }

    public Task Complete(Guid id, string handler, CancellationToken ct)
    {
        db.Idempotency.Add(
            new IdempotentMessage
            {
                MessageId = id,
                Handler = handler
            });

        return db.SaveChangesAsync(ct);
    }
}
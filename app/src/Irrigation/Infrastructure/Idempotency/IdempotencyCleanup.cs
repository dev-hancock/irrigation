using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Irrigation.Infrastructure.Idempotency;

public interface IIdempotencyCleanup
{
    Task Cleanup(CancellationToken cancellationToken);
}

public sealed class IdempotencyCleanup(IrrigationDbContext db, IOptions<IdempotencyOptions> options) : IIdempotencyCleanup
{
    private readonly IdempotencyOptions _options = options.Value;

    public async Task Cleanup(CancellationToken cancellationToken)
    {
        var cutoff = DateTimeOffset.UtcNow - _options.Retention;

        var expired = await db.Idempotency
            .Where(x => x.ProcessedAt < cutoff)
            .ToListAsync(cancellationToken);

        db.Idempotency.RemoveRange(expired);

        await db.SaveChangesAsync(cancellationToken);
    }
}
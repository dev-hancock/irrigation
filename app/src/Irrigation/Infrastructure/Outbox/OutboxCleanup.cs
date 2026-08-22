using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Irrigation.Infrastructure.Outbox
{
    public interface IOutboxCleanup
    {
        Task Cleanup(CancellationToken cancellationToken);
    }

    public class OutboxCleanup(IrrigationDbContext db, IOptions<OutboxOptions> options) : IOutboxCleanup
    {
        private readonly OutboxOptions _options = options.Value;

        public async Task Cleanup(CancellationToken cancellationToken)
        {
            var cutoff = DateTimeOffset.UtcNow - _options.Retention;

            var expired = await db.Outbox
                .Where(x => x.ProcessedAt < cutoff)
                .ToListAsync(cancellationToken);

            db.Outbox.RemoveRange(expired);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

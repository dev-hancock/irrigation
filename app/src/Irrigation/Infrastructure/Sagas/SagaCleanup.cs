using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Irrigation.Infrastructure.Sagas
{
    public interface ISagaCleanup
    {
        Task Cleanup(CancellationToken cancellationToken);
    }

    public class SagaCleanup(IrrigationDbContext db, IOptions<SagaOptions> options) : ISagaCleanup
    {
        private readonly SagaOptions _options = options.Value;

        public async Task Cleanup(CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;

            var sagas = await db.Sagas
                .Where(x =>
                    (x.CompletedAt != null &&
                     x.CompletedAt < now - _options.Retention) ||
                    (x.FailedAt != null &&
                     x.FailedAt < now - _options.Retention))
                .ToListAsync(cancellationToken);

            db.Sagas.RemoveRange(sagas);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

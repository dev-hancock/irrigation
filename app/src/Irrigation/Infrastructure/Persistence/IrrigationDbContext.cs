using System.Text.Json;
using Irrigation.Domain.Common;
using Irrigation.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Persistence
{
   

    public class IrrigationDbContext(DbContextOptions<IrrigationDbContext> options) : DbContext(options)
    {
        public DbSet<OutboxMessage> Outbox { get; set; }


        public override async Task<int> SaveChangesAsync(
            CancellationToken ct = default)
        {
            AddToOutbox();

            return await base.SaveChangesAsync(ct);
        }

        private void AddToOutbox()
        {
            var aggregates = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Where(x => x.Events.Count != 0)
                .ToArray();

            var notifications = aggregates
                .SelectMany(x => x.Events)
                .ToArray();

            foreach (var @event in notifications)
            {
                Outbox.Add(
                    new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        Type = @event.GetType().AssemblyQualifiedName!,
                        Data = JsonSerializer.Serialize(
                            @event,
                            @event.GetType()),
                        CreatedAt = DateTimeOffset.UtcNow
                    });
            }

            foreach (var aggregate in aggregates)
            {
                aggregate.ClearEvents();
            }
        }
    }
}

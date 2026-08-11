using System.Text.Json;
using Irrigation.Infrastructure.Persistence;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Outbox
{
    public class OutboxProcessor(IrrigationDbContext db, IMediator mediator)
    {
        public async Task Process(CancellationToken ct = default)
        {
            var messages = await db.Outbox
                .Where(x => x.ProcessedAt == null)
                .OrderBy(x => x.CreatedAt)
                .Take(20)
                .ToListAsync(ct);

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type);

                    if (type is null)
                    {
                        throw new InvalidOperationException($"Unable to resolve outbox type '{message.Type}'.");
                    }

                    var @event = (INotification) JsonSerializer.Deserialize(message.Data, type)!;

                    if (@event is null)
                    {
                        throw new InvalidOperationException($"Unable to deserialize outbox message '{message.Type}'.");
                    }

                    await mediator.Publish(@event, ct);

                    message.ProcessedAt = DateTimeOffset.UtcNow;
                    message.Error = null;
                }
                catch (Exception ex)
                {
                    message.Attempts++;
                    message.Error = ex.Message;
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}

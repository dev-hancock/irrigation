using System.Text.Json;
using Irrigation.Infrastructure.Persistence;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Outbox;

public interface IOutboxProcessor
{
    Task Process(CancellationToken ct = default);
}

public sealed partial class OutboxProcessor(
    IrrigationDbContext db,
    IMediator mediator,
    ILogger<OutboxProcessor> _) : IOutboxProcessor
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

                var @event = (INotification)JsonSerializer.Deserialize(message.Data, type)!;

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
                LogProcessingFailed(ex, message.Id, message.Type, message.Attempts + 1);

                message.Attempts++;
                message.Error = ex.Message;
            }
        }

        await db.SaveChangesAsync(ct);
    }

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to process outbox message '{MessageId}' of type '{MessageType}' on attempt '{Attempt}'.")]
    partial void LogProcessingFailed(
        Exception exception,
        Guid messageId,
        string messageType,
        int attempt);
}
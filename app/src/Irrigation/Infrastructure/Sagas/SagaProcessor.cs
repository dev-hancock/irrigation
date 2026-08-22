using System.Text.Json;
using Irrigation.Application.Common.Sagas;
using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Sagas;

public interface ISagaProcessor
{
    Task Process(CancellationToken ct = default);
}

public sealed partial class SagaProcessor(
    IrrigationDbContext db,
    IServiceProvider services,
    ILogger<SagaProcessor> _) : ISagaProcessor
{
    public async Task Process(CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        var sagas = await db.Sagas
            .Where(x =>
                x.CompletedAt == null &&
                x.FailedAt == null &&
                (x.NextAttemptAt == null || x.NextAttemptAt <= now))
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync(ct);

        foreach (var saga in sagas)
        {
            await Process(saga, ct);
        }

        await db.SaveChangesAsync(ct);
    }

    private async Task Process(SagaInstance saga, CancellationToken ct)
    {
        try
        {
            var type = Type.GetType(saga.Type);

            if (type is null)
            {
                throw new InvalidOperationException($"Unable to resolve saga type '{saga.Type}'.");
            }

            var state = JsonSerializer.Deserialize(saga.Data, type);

            if (state is null)
            {
                throw new InvalidOperationException($"Unable to deserialize saga state '{saga.Type}'.");
            }

            var handler = GetHandler(type);

            var result = await handler.Handle(
                state,
                new SagaContext
                {
                    Id = saga.Id, Attempts = saga.Attempts
                },
                ct);

            saga.Data = JsonSerializer.Serialize(state, type);
            saga.UpdatedAt = DateTimeOffset.UtcNow;
            saga.Error = null;

            switch (result)
            {
                case SagaResult.Completed:
                {
                    saga.CompletedAt = DateTimeOffset.UtcNow;
                    break;
                }
                case SagaResult.Retry retry:
                {
                    saga.Attempts++;
                    saga.NextAttemptAt = retry.At;
                    break;
                }
                case SagaResult.Failed failed:
                {
                    saga.FailedAt = DateTimeOffset.UtcNow;
                    saga.Error = failed.Error;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            LogProcessingFailed(ex, saga.Id, saga.Type, saga.Attempts + 1);

            saga.Attempts++;
            saga.Error = ex.Message;
            saga.NextAttemptAt = DateTimeOffset.UtcNow.AddSeconds(5);
        }
    }

    private Type GetHandlerType(Type type)
    {
        return typeof(ISagaHandler<>).MakeGenericType(type);
    }

    private ISagaHandler GetHandler(Type type)
    {
        return (ISagaHandler)services.GetRequiredService(GetHandlerType(type));
    }

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to process saga '{SagaId}' of type '{SagaType}' on attempt '{Attempt}'.")]
    partial void LogProcessingFailed(
        Exception exception,
        Guid sagaId,
        string sagaType,
        int attempt);
}

using Irrigation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Irrigation.Infrastructure.Saga
{
    public class SagaWorker(IServiceScopeFactory factory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = factory.CreateScope();

                var processor = scope.ServiceProvider.GetRequiredService<ISagaProcessor>();

                await processor.Process(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

    public interface ISagaProcessor
    {
        Task Process(CancellationToken ct = default);
    }

    public sealed class SagaProcessor(IrrigationDbContext db, IServiceProvider services) : ISagaProcessor
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

        private async Task Process(SagaState saga, CancellationToken ct)
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

                var result = await handler.Handle(state, ct);

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
                saga.Attempts++;
                saga.Error = ex.Message;
                saga.FailedAt = DateTimeOffset.UtcNow;
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
    }

    public interface ISagaHandler
    {
        Task<SagaResult> Handle(object state, CancellationToken ct = default);
    }

    public interface ISagaHandler<in TState> : ISagaHandler
    {
        Task<SagaResult> Handle(TState state, CancellationToken ct = default);

        Task<SagaResult> ISagaHandler.Handle(object state, CancellationToken ct)
        {
            return Handle((TState)state, ct);
        }
    }

    public interface ISagaStore
    {
        Task<Guid> Start<TState>(TState state, CancellationToken ct = default);
    }

    public class SagaState
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Type { get; init; }

        public required string Data { get; set; }

        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? NextAttemptAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }

        public DateTimeOffset? FailedAt { get; set; }

        public string? Error { get; set; }

        public int Attempts { get; set; }

        public bool Completed => CompletedAt.HasValue;

        public bool Failed => FailedAt.HasValue;

        public bool Resolved => Completed || Failed;
    }

    public abstract record SagaResult
    {
        private SagaResult() {}

        public sealed record Completed : SagaResult;

        public sealed record Retry(DateTimeOffset At) : SagaResult;

        public sealed record Failed(string Error) : SagaResult;
    }

    public sealed class SagaStore(IrrigationDbContext db) : ISagaStore
    {
        public async Task<Guid> Start<TState>(TState state, CancellationToken ct = default)
        {
            var saga = new SagaState
            {
                Type = typeof(TState).AssemblyQualifiedName!,
                Data = JsonSerializer.Serialize(state)
            };

            db.Sagas.Add(saga);

            await db.SaveChangesAsync(ct);

            return saga.Id;
        }
    }
}

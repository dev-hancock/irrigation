using Microsoft.Extensions.Options;

namespace Irrigation.Infrastructure.Idempotency;

public sealed class IdempotencyWorker(IServiceScopeFactory factory, IOptions<IdempotencyOptions> options) : BackgroundService
{
    private readonly IdempotencyOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();

            var cleanup = scope.ServiceProvider.GetRequiredService<IIdempotencyCleanup>();

            await cleanup.Cleanup(stoppingToken);

            await Task.Delay(_options.CleanupInterval, stoppingToken);
        }
    }
}
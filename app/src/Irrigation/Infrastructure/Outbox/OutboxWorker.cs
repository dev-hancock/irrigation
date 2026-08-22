namespace Irrigation.Infrastructure.Outbox;

public sealed class OutboxWorker(IServiceScopeFactory factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timestamp = DateTimeOffset.MinValue;

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

            await processor.Process(stoppingToken);

            if (DateTimeOffset.UtcNow - timestamp > TimeSpan.FromHours(1))
            {
                var cleanup = scope.ServiceProvider.GetRequiredService<IOutboxCleanup>();

                await cleanup.Cleanup(stoppingToken);

                timestamp = DateTimeOffset.UtcNow;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}

public interface IOutboxStore
{
    void IsProcessed(Guid id);

    void Complete(Guid id);


}
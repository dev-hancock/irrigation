namespace Irrigation.Infrastructure.Outbox;

public sealed class OutboxWorker(IServiceScopeFactory factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

            await processor.Process(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
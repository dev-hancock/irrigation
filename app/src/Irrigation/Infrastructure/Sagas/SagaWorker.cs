namespace Irrigation.Infrastructure.Sagas;

public class SagaWorker(IServiceScopeFactory factory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timestamp = DateTimeOffset.MinValue;

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<ISagaProcessor>();

            await processor.Process(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

            if (DateTimeOffset.UtcNow - timestamp > TimeSpan.FromHours(1))
            {
                var cleanup = scope.ServiceProvider.GetRequiredService<ISagaCleanup>();

                await cleanup.Cleanup(stoppingToken);

                timestamp = DateTimeOffset.UtcNow;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
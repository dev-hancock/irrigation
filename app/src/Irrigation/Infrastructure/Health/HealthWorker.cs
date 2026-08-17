using Irrigation.Application.Health.Commands;
using Mediator;
using Microsoft.Extensions.Options;

namespace Irrigation.Infrastructure.Health;

public class HealthWorker(IServiceScopeFactory factory, IOptions<HealthOptions> options) : BackgroundService
{
    private readonly HealthOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_options.Heartbeat);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = factory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new SendHeartbeatCommand(), stoppingToken);

            await mediator.Send(new CheckHealthCommand(), stoppingToken);
        }
    }
}
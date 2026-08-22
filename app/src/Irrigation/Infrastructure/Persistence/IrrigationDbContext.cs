using System.Text.Json;
using Irrigation.Domain.Activities;
using Irrigation.Domain.Common;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Valves;
using Irrigation.Infrastructure.Idempotency;
using Irrigation.Infrastructure.Outbox;
using Irrigation.Infrastructure.Sagas;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure.Persistence;

public class IrrigationDbContext(DbContextOptions<IrrigationDbContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities { get; set; }

    public DbSet<Valve> Valves { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<OutboxMessage> Outbox { get; set; }

    public DbSet<IdempotentMessage> Idempotency { get; set; }

    public DbSet<SagaInstance> Sagas { get; set; }

    public override async Task<int> SaveChangesAsync(
        CancellationToken ct = default)
    {
        AddToOutbox();

        return await base.SaveChangesAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IrrigationDbContext).Assembly);
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
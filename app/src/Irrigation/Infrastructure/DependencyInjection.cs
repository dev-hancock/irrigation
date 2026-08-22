using Irrigation.Application.Activities;
using Irrigation.Application.Activities.Abstractions;
using Irrigation.Application.Activities.Queries;
using Irrigation.Application.Common;
using Irrigation.Application.Common.Sagas;
using Irrigation.Application.Health;
using Irrigation.Application.Valves;
using Irrigation.Application.Valves.Sagas;
using Irrigation.Infrastructure.Devices;
using Irrigation.Infrastructure.Health;
using Irrigation.Infrastructure.Idempotency;
using Irrigation.Infrastructure.Mqtt;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Irrigation.Infrastructure.Outbox;
using Irrigation.Infrastructure.Persistence;
using Irrigation.Infrastructure.Sagas;
using Irrigation.Infrastructure.Valves;
using Mediator;
using Microsoft.EntityFrameworkCore;
using MQTTnet;

namespace Irrigation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDb();
        services.AddIdempotency();
        services.AddOutbox();
        services.AddSagas();
        services.AddMqtt();

        services.AddValves();
        services.AddDevices();
        services.AddActivities();
        services.AddHealth();

        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentNotificationHandler<>));
        services.Decorate(typeof(IMessageHandler), typeof(IdempotentMessageHandler));

        services.AddSingleton<IEventBus, EventBus>();

        return services;
    }

    private static IServiceCollection AddIdempotency(this IServiceCollection services)
    {
        services.AddScoped<IIdempotencyHandler, IdempotencyHandler>();
        services.AddScoped<IIdempotencyCleanup, IdempotencyCleanup>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        
        services.AddOptions<IdempotencyOptions>().BindConfiguration(IdempotencyOptions.Section);

        services.AddHostedService<IdempotencyWorker>();

        return services;
    }

    private static IServiceCollection AddMqtt(this IServiceCollection services)
    {
        services.AddSingleton<IMqttClient>(_ =>
        {
            var factory = new MqttClientFactory();

            return factory.CreateMqttClient();
        });

        services.AddScoped<IMqttConnection, MqttConnection>();
        services.AddScoped<IMqttConsumer, MqttConsumer>();
        services.AddScoped<IMqttPublisher, MqttPublisher>();

        services.AddOptions<MqttOptions>().BindConfiguration(MqttOptions.Section);

        services.AddHostedService<MqttService>();

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services)
    {
        services.AddDbContext<IrrigationDbContext>(opt => opt.UseInMemoryDatabase("Irrigation"));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddScoped<IOutboxProcessor, OutboxProcessor>();
        services.AddScoped<IOutboxCleanup, OutboxCleanup>();

        services.AddOptions<OutboxOptions>().BindConfiguration(OutboxOptions.Section);

        services.AddHostedService<OutboxWorker>();

        return services;
    }

    private static IServiceCollection AddSagas(this IServiceCollection services)
    {
        services.AddScoped<ISagaStore, SagaStore>();
        services.AddScoped<ISagaProcessor, SagaProcessor>();
        services.AddScoped<ISagaCleanup, SagaCleanup>();

        services.AddOptions<SagaOptions>().BindConfiguration(SagaOptions.Section);

        services.AddHostedService<SagaWorker>();

        return services;
    }

    private static IServiceCollection AddValves(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler, ValveMessageHandler>();

        services.AddScoped<ISagaHandler<ValveOperationState>, ValveOperationSaga>();

        services.AddScoped<IValveController, ValveController>();

        return services;
    }

    private static IServiceCollection AddDevices(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler, DeviceMessageHandler>();

        return services;
    }

    private static IServiceCollection AddActivities(this IServiceCollection services)
    {
        services.AddScoped<IActivityMapper, ActivityMapper>();

        services.AddScoped<IActivityWriter, ActivityWriter>();

        return services;
    }

    private static IServiceCollection AddHealth(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler, HealthMessageHandler>();

        services.AddOptions<HealthOptions>().BindConfiguration(HealthOptions.Section);

        services.AddScoped<IHealthService, HealthService>();

        services.AddHostedService<HealthWorker>();

        return services;
    }
}
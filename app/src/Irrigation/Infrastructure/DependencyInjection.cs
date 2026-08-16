using Irrigation.Application.Common;
using Irrigation.Application.Health;
using Irrigation.Application.Valves;
using Irrigation.Domain.Repository;
using Irrigation.Infrastructure.Devices;
using Irrigation.Infrastructure.Health;
using Irrigation.Infrastructure.Mqtt;
using Irrigation.Infrastructure.Mqtt.Abstraction;
using Irrigation.Infrastructure.Outbox;
using Irrigation.Infrastructure.Persistence;
using Irrigation.Infrastructure.Valves;
using Microsoft.EntityFrameworkCore;
using MQTTnet;

namespace Irrigation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDb();
        services.AddOutbox();
        services.AddMqtt();

        services.AddValves();
        services.AddDevices();
        services.AddHealth();

        services.AddMediator(opt => opt.ServiceLifetime = ServiceLifetime.Scoped);

        services.AddSingleton<IEventBus, EventBus>();

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

        services.AddHostedService<OutboxWorker>();

        return services;
    }

    private static IServiceCollection AddValves(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler, ValveMessageHandler>();

        services.AddScoped<IValveService, ValveService>();

        return services;
    }

    private static IServiceCollection AddDevices(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler, DeviceMessageHandler>();

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
using Irrigation.Application.Common;
using Irrigation.Application.Valves;
using Irrigation.Domain.Repository;
using Irrigation.Infrastructure.Mqtt;
using Irrigation.Infrastructure.Outbox;
using Irrigation.Infrastructure.Persistence;
using Irrigation.Infrastructure.Ports.Valves;
using Microsoft.EntityFrameworkCore;

namespace Irrigation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDb();
            services.AddOutbox();
            services.AddMqtt();

            services.AddValves();

            services.AddMediator(opt => opt.ServiceLifetime = ServiceLifetime.Scoped);

            services.AddScoped<IEventBus, EventBus>();

            return services;
        }

        private static IServiceCollection AddMqtt(this IServiceCollection services)
        {
            services.AddSingleton<IMqttClient, MqttClient>();

            services.AddScoped<IMqttConsumer, MqttConsumer>();

            services.AddHostedService<MqttService>();

            return services;
        }

        private static IServiceCollection AddDb(this IServiceCollection services)
        {
            services.AddDbContext<IrrigationDbContext>(opt => opt.UseInMemoryDatabase("Irrigation"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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
    }
}

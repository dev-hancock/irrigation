using Irrigation.Domain.Common;

namespace Irrigation.Application.Common
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event);

        IDisposable Subscribe<T>(Func<T, Task> handler);
    }

    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = [];

        public IDisposable Subscribe<T>(
            Func<T, Task> handler)
        {
            var type = typeof(T);

            if (!_handlers.TryGetValue(type, out var handlers))
            {
                handlers = [];
                _handlers[type] = handlers;
            }

            handlers.Add(handler);

            return new Subscription(
                () => handlers.Remove(handler));
        }

        public async Task PublishAsync<T>(T @event)
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers))
            {
                return;
            }

            foreach (var handler in handlers.ToArray())
            {
                await ((Func<T, Task>)handler)(@event);
            }
        }

        private sealed class Subscription(Action dispose) : IDisposable
        {
            public void Dispose() => dispose();
        }
    }

    public interface INotificationHandler<T> where T : INotification
    {
        Task Handle(T notification, CancellationToken ct = default);
    }
    public interface IRequestHandler<T> where T : IRequest
    {
        Task Handle(T request, CancellationToken ct = default);
    }


    public interface IMediator
    {
        Task Publish<T>(T notification, CancellationToken ct = default) where T : INotification;

        Task Send<T>(T request, CancellationToken ct = default) where T : IRequest;
    }
}

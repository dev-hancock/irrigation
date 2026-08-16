namespace Irrigation.Application.Common;

public interface IEventBus
{
    Task Publish<T>(T @event, CancellationToken ct = default);

    IDisposable Subscribe<T>(Func<T, CancellationToken, Task> handler);
}

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];
    private readonly Lock _lock = new();

    public IDisposable Subscribe<T>(Func<T, CancellationToken, Task> handler)
    {
        var type = typeof(T);

        lock (_lock)
        {
            if (!_handlers.TryGetValue(type, out var handlers))
            {
                handlers = [];
                _handlers[type] = handlers;
            }

            handlers.Add(handler);
        }

        return new Subscription(() => Unsubscribe(type, handler));
    }

    public async Task Publish<T>(T @event, CancellationToken ct = default)
    {
        Delegate[] handlers;

        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(T), out var registeredHandlers))
            {
                return;
            }

            handlers = registeredHandlers.ToArray();
        }

        foreach (var handler in handlers)
        {
            ct.ThrowIfCancellationRequested();

            await ((Func<T, CancellationToken, Task>)handler)(@event, ct);
        }
    }

    private void Unsubscribe(Type type, Delegate handler)
    {
        lock (_lock)
        {
            if (!_handlers.TryGetValue(type, out var handlers))
            {
                return;
            }

            handlers.Remove(handler);

            if (handlers.Count == 0)
            {
                _handlers.Remove(type);
            }
        }
    }

    private sealed class Subscription(Action dispose) : IDisposable
    {
        public void Dispose()
        {
            dispose();
        }
    }
}
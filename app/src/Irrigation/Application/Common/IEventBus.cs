namespace Irrigation.Application.Common;

public interface IEventBus
{
    Task Publish<T>(T @event, CancellationToken ct = default);

    IDisposable Subscribe<T>(Func<T, CancellationToken, Task> handler);
}

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

    public IDisposable Subscribe<T>(Func<T, CancellationToken, Task> handler)
    {
        var type = typeof(T);

        if (!_handlers.TryGetValue(type, out var handlers))
        {
            handlers = [];
            _handlers[type] = handlers;
        }

        handlers.Add(handler);

        return new Subscription(() => handlers.Remove(handler));
    }

    public async Task Publish<T>(T @event, CancellationToken ct = default)
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers.ToArray())
        {
            ct.ThrowIfCancellationRequested();

            await ((Func<T, CancellationToken, Task>)handler)(@event, ct);
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
namespace Irrigation.Infrastructure.Idempotency;

public interface IIdempotencyHandler
{
    ValueTask Execute(
        Guid id,
        Type consumer,
        Func<ValueTask> handle,
        CancellationToken cancellationToken);
}

public sealed class IdempotencyHandler(IIdempotencyStore store) : IIdempotencyHandler
{
    public async ValueTask Execute(
        Guid id,
        Type consumer,
        Func<ValueTask> handle,
        CancellationToken cancellationToken)
    {
        var handler = consumer.FullName;

        if (handler == null)
        {
            throw new InvalidOperationException($"Unable to resolve handler name for '{consumer}'");
        }

        var processed = await store.IsProcessed(id, handler, cancellationToken);

        if (processed)
        {
            return;
        }

        await handle();
        
        await store.Complete(id, handler, cancellationToken);
    }
}
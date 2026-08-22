using ErrorOr;
using Irrigation.Infrastructure.Mqtt.Abstraction;

namespace Irrigation.Infrastructure.Idempotency;

public sealed class IdempotentMessageHandler(IMessageHandler inner, IIdempotencyHandler idempotency) : IMessageHandler
{
    public bool CanHandle(Message message)
    {
        return inner.CanHandle(message);
    }

    public ValueTask Handle(Message message, CancellationToken cancellationToken)
    {
        var consumer = inner.GetType();

        ValueTask HandleInner()
        {
            return inner.Handle(message, cancellationToken);
        }

        return idempotency.Execute(
            message.EventId,
            consumer,
            HandleInner,
            cancellationToken);
    }
}
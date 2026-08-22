using Irrigation.Domain.Shared;
using Mediator;

namespace Irrigation.Infrastructure.Idempotency;

public sealed class IdempotentNotificationHandler<T>(INotificationHandler<T> inner, IIdempotencyHandler idempotency) 
    : INotificationHandler<T> where T : INotification, IIdentified
{
    public ValueTask Handle(T notification, CancellationToken cancellationToken)
    {
        var consumer = inner.GetType();

        ValueTask HandleInner()
        {
            return inner.Handle(notification, cancellationToken);
        }

        return idempotency.Execute(
            notification.EventId,
            consumer,
            HandleInner,
            cancellationToken);
    }
}
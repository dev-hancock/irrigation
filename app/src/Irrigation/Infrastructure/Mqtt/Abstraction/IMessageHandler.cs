using ErrorOr;

namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMessageHandler
{
    bool CanHandle(Message message);

    Task<ErrorOr<Success>> Handle(Message message, CancellationToken ct);
}
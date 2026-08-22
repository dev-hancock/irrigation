namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMessageHandler
{
    bool CanHandle(Message message);

    ValueTask Handle(Message message, CancellationToken cancellationToken);
}
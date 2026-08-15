using ErrorOr;

namespace Irrigation.Infrastructure.Mqtt.Abstraction;

public interface IMqttConnection
{
    bool IsConnected { get; }

    Task<ErrorOr<Success>> Connect(CancellationToken ct = default);

    Task<ErrorOr<Success>> Subscribe(string pattern, CancellationToken ct = default);
}
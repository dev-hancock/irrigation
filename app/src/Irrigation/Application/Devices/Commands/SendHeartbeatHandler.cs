using ErrorOr;
using Irrigation.Application.Health;
using Mediator;

namespace Irrigation.Application.Devices.Commands;

public class SendHeartbeatCommand : IRequest<ErrorOr<Success>>;

public class SendHeartbeatHandler(IHealthService health) : IRequestHandler<SendHeartbeatCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(SendHeartbeatCommand request, CancellationToken cancellationToken)
    {
        await health.Heartbeat(cancellationToken);

        return Result.Success;
    }
}
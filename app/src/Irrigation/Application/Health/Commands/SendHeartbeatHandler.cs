using ErrorOr;
using Mediator;

namespace Irrigation.Application.Health.Commands;

public class SendHeartbeatCommand : IRequest<ErrorOr<Success>>;

public class SendHeartbeatHandler(IHealthService health) : IRequestHandler<SendHeartbeatCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(SendHeartbeatCommand request, CancellationToken cancellationToken)
    {
        return await health.Heartbeat(cancellationToken);
    }
}
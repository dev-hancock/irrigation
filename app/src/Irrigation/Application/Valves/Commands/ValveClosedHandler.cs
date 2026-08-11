using ErrorOr;
using Mediator;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ValveClosedCommand : IRequest<ErrorOr<Success>>
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public class ValveClosedHandler : IRequestHandler<ValveClosedCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(ValveClosedCommand command, CancellationToken ct = default)
        {


        }
    }
}

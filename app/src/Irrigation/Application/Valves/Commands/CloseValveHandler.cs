using ErrorOr;
using Mediator;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record CloseValveCommand : IRequest<ErrorOr<Success>>
    {
        public string Device { get; set; }

        public string Id { get; set; }
    }

    public sealed class CloseValveHandler: IRequestHandler<CloseValveCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(CloseValveCommand command, CancellationToken ct = default)
        {


        }
    }
}

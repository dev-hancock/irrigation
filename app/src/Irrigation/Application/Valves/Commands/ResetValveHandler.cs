using ErrorOr;
using Mediator;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ResetValveCommand : IRequest<ErrorOr<Success>>
    {
        public string? Device { get; set; }
    }

    public sealed class ResetValveHandler : IRequestHandler<ResetValveCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(ResetValveCommand command, CancellationToken ct = default)
        {


        }
    }
}

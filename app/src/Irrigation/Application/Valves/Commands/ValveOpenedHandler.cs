using Irrigation.Application.Common;
using Irrigation.Domain.Common;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ValveOpenedCommand : IRequest
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public class ValveOpenedHandler : IRequestHandler<ValveOpenedCommand>
    {
        public Task Handle(ValveOpenedCommand request, CancellationToken ct = default)
        {
            // Handle the command
            return Task.CompletedTask;
        }
    }
}

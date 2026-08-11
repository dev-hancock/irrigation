using Irrigation.Domain.Common;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ValveClosedCommand : IRequest
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public class ValveClosedHandler
    {
    }
}

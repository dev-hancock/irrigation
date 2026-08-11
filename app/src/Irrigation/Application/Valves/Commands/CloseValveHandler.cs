using ErrorOr;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record CloseValveCommand
    {
        public string Device { get; set; }

        public string Id { get; set; }
    }

    public sealed class CloseValveHandler
    {
        public async Task<ErrorOr<Success>> Handle(CloseValveCommand command, CancellationToken ct)
        {


        }
    }
}

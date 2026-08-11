using ErrorOr;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ResetValveCommand
    {
        public string? Device { get; set; }
    }

    public sealed class ResetValveHandler
    {
        public async Task<ErrorOr<Success>> Handle(ResetValveCommand command, CancellationToken ct)
        {


        }
    }
}

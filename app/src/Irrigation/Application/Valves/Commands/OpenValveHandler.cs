using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record OpenValveCommand
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public sealed class OpenValveHandler(IRepository<Valve> repo)
    {
        public async Task<ErrorOr<Success>> Handle(OpenValveCommand command, CancellationToken ct)
        {
            var spec = new GetValveSpec(command.Device, command.Id);

            var valve = await repo.FirstOrDefaultAsync(spec, ct).ConfigureAwait(false);

            if (valve is null)
            {
                return Error.NotFound("Valve.NotFound", $"Valve with device '{command.Device}' and id '{command.Id}' not found.");
            }

            valve.Open();

            await repo.SaveChangesAsync(ct).ConfigureAwait(false);

            return Result.Success;
        }
    }
}

using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Common;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record ValveOpenedCommand : IRequest<ErrorOr<Success>>
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public class ValveOpenedHandler(IRepository<Valve> repo) : IRequestHandler<ValveOpenedCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(ValveOpenedCommand request, CancellationToken ct = default)
        {
            var spec = new GetValveSpec(request.Device, request.Id);

            var valve = await repo.FirstOrDefaultAsync(spec, ct);

            if (valve is null)
            {
                return Error.NotFound("Valve.NotFound", $"Valve with device '{request.Device}' and id '{request.Id}' not found.");
            }

            valve.Opened();

            await repo.SaveChangesAsync(ct);

            return Result.Success;
        }
    }
}

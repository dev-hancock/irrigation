using ErrorOr;
using Irrigation.Application.Common;
using Irrigation.Domain.Common;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands
{
    public sealed record OpenValveCommand : IRequest<ErrorOr<Success>>
    {
        public required string Device { get; set; }

        public required string Id { get; set; }
    }

    public sealed class OpenValveHandler(IRepository<Valve> repo) : IRequestHandler<OpenValveCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(OpenValveCommand request, CancellationToken ct = default)
        {
            var spec = new GetValveSpec(request.Device, request.Id);

            var valve = await repo.FirstOrDefaultAsync(spec, ct);

            if (valve is null)
            {
                return Error.NotFound("Valve.NotFound", $"Valve with device '{request.Device}' and id '{request.Id}' not found.");
            }

            valve.Open();

            await repo.SaveChangesAsync(ct);

            return Result.Success;
        }
    }
}

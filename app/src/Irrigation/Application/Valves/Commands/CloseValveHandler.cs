using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record CloseValveCommand : IRequest<ErrorOr<Success>>
{
    public required string Device { get; set; }

    public required string Id { get; set; }
}

public sealed class CloseValveHandler(IRepository<Valve> repo) : IRequestHandler<CloseValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CloseValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetValveSpec(request.Device, request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with device '{request.Device}' and id '{request.Id}' not found.");
        }

        valve.Close();

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
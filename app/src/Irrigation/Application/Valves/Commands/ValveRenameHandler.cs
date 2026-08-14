using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record ValveRenameCommand : IRequest<ErrorOr<Success>>
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
}

public class ValveRenameHandler(IRepository<Valve> repo) : IRequestHandler<ValveRenameCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(ValveRenameCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetValveSpec(request.Id);

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id}' not found.");
        }

        valve.Rename(request.Name);

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
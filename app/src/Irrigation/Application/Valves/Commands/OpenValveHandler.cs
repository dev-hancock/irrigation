using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record OpenValveCommand : IRequest<ErrorOr<Success>>
{
    public required Guid Id { get; set; }
}

public sealed class OpenValveHandler(IRepository<Valve> repo) : IRequestHandler<OpenValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(OpenValveCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetValveSpec(ValveId.From(request.Id));

        var valve = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id}' not found.");
        }

        valve.Open();

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
using ErrorOr;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Commands;

public sealed record CloseValveCommand : IRequest<ErrorOr<Success>>
{
    public required Guid Id { get; set; }
}

public sealed class CloseValveHandler(IRepository<Valve> repo) : IRequestHandler<CloseValveCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(CloseValveCommand request, CancellationToken cancellationToken)
    {
        var valve = await repo.FirstOrDefaultAsync(
            new GetValveSpec(ValveId.From(request.Id)),
            cancellationToken);

        if (valve is null)
        {
            return Error.NotFound("Valve.NotFound", $"Valve with id '{request.Id}' not found.");
        }

        valve.Close();

        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
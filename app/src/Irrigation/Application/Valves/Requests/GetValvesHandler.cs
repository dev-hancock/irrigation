using ErrorOr;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Repository;
using Irrigation.Domain.Specifications;
using Irrigation.Domain.Valves;
using Mediator;

namespace Irrigation.Application.Valves.Requests
{
    public class ValveDto
    {
        public Guid Id { get; set; }

        public string HardwareId { get; set; }

        public Guid DeviceId { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Updated { get; set; }
    }

    public class GetValvesRequest : IRequest<ErrorOr<ValveDto[]>>
    {
        public DeviceId? Device { get; set; }
    }
    
    public class GetValvesHandler(IRepository<Valve> repo) : IRequestHandler<GetValvesRequest, ErrorOr<ValveDto[]>>
    {
        public async ValueTask<ErrorOr<ValveDto[]>> Handle(GetValvesRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetValvesSpec(request.Device);

            var valves = await repo.ListAsync(spec, cancellationToken);

            return valves
                .Select(x => new ValveDto
                {
                    Id = x.Id.Value,
                    Name = x.Name
                })
                .ToArray();
        }
    }
}

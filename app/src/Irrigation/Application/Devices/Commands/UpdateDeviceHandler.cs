using ErrorOr;
using Mediator;

namespace Irrigation.Application.Devices.Commands
{

    public class UpdateDeviceCommand : IRequest<ErrorOr<Success>>
    {
        public required string Id { get; set; }

        public required string Firmware { get; set; }

        public required string Model { get; set; }

        public required string Version { get; set; }
    }

    public class UpdateDeviceHandler : IRequestHandler<UpdateDeviceCommand, ErrorOr<Success>>
    {
        public ValueTask<ErrorOr<Success>> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

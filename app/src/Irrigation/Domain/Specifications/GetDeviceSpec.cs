using Ardalis.Specification;
using Irrigation.Domain.Devices;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Specifications;

public class GetDeviceSpec : Specification<Device>
{
    public GetDeviceSpec(DeviceId id)
    {
    }

    public GetDeviceSpec(HardwareId id)
    {
    }
}
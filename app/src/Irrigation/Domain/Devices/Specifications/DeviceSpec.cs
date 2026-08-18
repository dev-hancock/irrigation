using Ardalis.Specification;
using Irrigation.Domain.Shared;

namespace Irrigation.Domain.Devices.Specifications;

public class DeviceSpec : Specification<Device>
{
    public DeviceSpec(DeviceId id)
    {
        Query.Where(x => x.Id == id);
    }

    public DeviceSpec(HardwareId id)
    {
        Query.Where(x => x.HardwareId == id);
    }
}

public class DeviceNotSeenSinceSpec : Specification<Device>
{
    public DeviceNotSeenSinceSpec(DateTimeOffset cutoff)
    {
        Query.Where(x => x.UpdatedAt <= cutoff);
    }
}
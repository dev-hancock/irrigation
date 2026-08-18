using Irrigation.Domain.Valves;

namespace Irrigation.Application.Valves.Queries;

public class ValveModel
{
    public Guid Id { get; set; }

    public int Index { get; set; }

    public Guid DeviceId { get; set; }

    public string Status { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Updated { get; set; }

    public static ValveModel From(Valve valve)
    {
        return new ValveModel
        {
            Id = valve.Id.Value,
            Name = valve.Name,
            DeviceId = valve.DeviceId,
            Index = valve.Index,
            Status = valve.Status.ToString(),
            Updated = valve.UpdatedAt
        };
    }
}
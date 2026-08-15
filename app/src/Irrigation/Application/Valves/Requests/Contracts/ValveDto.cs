namespace Irrigation.Application.Valves.Requests.Contracts;

public class ValveDto
{
    public Guid Id { get; set; }

    public string HardwareId { get; set; }

    public Guid DeviceId { get; set; }

    public string Status { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Updated { get; set; }
}
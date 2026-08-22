namespace Irrigation.Domain.Shared;

public interface IIdentified
{
    Guid EventId { get; }
}
namespace Irrigation.Application.Activities.Abstractions
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
    }
}

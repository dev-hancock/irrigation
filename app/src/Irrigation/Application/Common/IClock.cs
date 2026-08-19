namespace Irrigation.Application.Common
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
    }
}

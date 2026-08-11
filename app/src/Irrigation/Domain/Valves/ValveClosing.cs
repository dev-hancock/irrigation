using Mediator;

namespace Irrigation.Domain.Valves
{
    public sealed record ValveClosing : INotification
    {
        public ValveId Id { get; internal set; }
    }
}

using Irrigation.Domain.Common;

namespace Irrigation.Domain.Valves
{
    public sealed record ValveOpening : INotification
    {
        public ValveId Id { get; internal set; }
    }
}

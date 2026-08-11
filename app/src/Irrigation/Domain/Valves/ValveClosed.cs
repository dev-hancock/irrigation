using Irrigation.Domain.Common;

namespace Irrigation.Domain.Valves
{
    public sealed record ValveClosed : INotification
    {
        public ValveId Id { get; internal set; }
    }
}

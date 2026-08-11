using Irrigation.Domain.Common;

namespace Irrigation.Domain.Valves
{
    public sealed record ValveOpened : INotification
    {
        public ValveId Id { get; internal set; }
    }
}

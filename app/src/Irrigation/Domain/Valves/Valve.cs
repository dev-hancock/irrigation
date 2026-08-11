using Irrigation.Domain.Common;

namespace Irrigation.Domain.Valves
{
    public record struct ValveId(string Value);

    public class Valve : AggregateRoot
    {
        public ValveState State { get; private set; }

        public string Name { get; private set; }

        public ValveId Id { get; private set; }

        public void Open()
        {
            if (State is ValveState.Opened or ValveState.Opening)
            {
                return;
            }

            State = ValveState.Opening;

            AddNotification(new ValveOpening
            {
                Id = Id
            });
        }

        public void Update(string name)
        {
            Name = name;
        }

        public void Opened()
        {
            if (State is ValveState.Opened)
            {
                return;
            }

            State = ValveState.Opened;

            AddNotification(new ValveOpened
            {
                Id = Id
            });
        }

        public void Close()
        {
            if (State is ValveState.Closed or ValveState.Closing)
            {
                return;
            }

            State = ValveState.Closing;

            AddNotification(new ValveClosing
            {
                Id = Id
            });
        }

        public void Closed()
        {
            if (State is ValveState.Closed)
            {
                return;
            }

            State = ValveState.Closed;

            AddNotification(new ValveClosed
            {
                Id = Id
            });
        }
    }
}

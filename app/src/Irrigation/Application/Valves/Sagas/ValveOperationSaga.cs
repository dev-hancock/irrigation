using Irrigation.Application.Common;
using Irrigation.Application.Common.Sagas;
using Irrigation.Domain.Devices.Specifications;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;

namespace Irrigation.Application.Valves.Sagas;

public class ValveOperationSaga(IUnitOfWork uow, IValveController controller) : ISagaHandler<ValveOperationState>
{
    private const int MaxAttempts = 3;

    public async Task<SagaResult> Handle(ValveOperationState state, SagaContext context, CancellationToken ct = default)
    {
        var valve = await uow.Valves.FirstOrDefaultAsync(
            new ValveSpec(state.ValveId),
            ct);

        if (valve is null)
        {
            return Saga.Fail($"Valve '{state.ValveId}' not found.");
        }

        if (valve.Status == state.Target)
        {
            switch (state.Target)
            {
                case ValveStatus.Open:
                    valve.Opened(state.Origin);
                    break;

                case ValveStatus.Closed:
                    valve.Closed(state.Origin);
                    break;

                default:
                    return Saga.Fail(
                        $"Unsupported target state '{state.Target}'.");
            }

            await uow.SaveChangesAsync(ct);

            return Saga.Complete();
        }

        if (context.Attempts >= MaxAttempts)
        {
            valve.Fault();

            await uow.SaveChangesAsync(ct);

            return Saga.Fail($"Valve '{state.ValveId}' failed to reach '{state.Target}'.");
        }

        var device = await uow.Devices.FirstOrDefaultAsync(
            new DeviceSpec(valve.DeviceId),
            ct);

        if (device is null)
        {
            return Saga.Fail($"Device '{valve.DeviceId}' not found.");
        }

        switch (state.Target)
        {
            case ValveStatus.Open:
                await controller.Open(
                    valve.Index,
                    device.HardwareId,
                    ct);
                break;

            case ValveStatus.Closed:
                await controller.Close(
                    valve.Index,
                    device.HardwareId,
                    ct);
                break;

            default:
                return Saga.Fail($"Unsupported target state '{state.Target}'.");
        }

        return Saga.RetryAfter(TimeSpan.FromSeconds(5));
    }
}
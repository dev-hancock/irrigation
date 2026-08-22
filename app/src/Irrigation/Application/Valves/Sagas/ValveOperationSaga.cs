using Irrigation.Application.Common;
using Irrigation.Application.Common.Sagas;
using Irrigation.Domain.Devices.Specifications;
using Irrigation.Domain.Valves;
using Irrigation.Domain.Valves.Specifications;

namespace Irrigation.Application.Valves.Sagas;

public class ValveOperationSaga(IUnitOfWork uow) : ISagaHandler<ValveOperationState>
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

        return Saga.RetryAfter(TimeSpan.FromSeconds(5));
    }
}
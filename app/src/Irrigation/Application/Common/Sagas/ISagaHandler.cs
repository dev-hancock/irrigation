namespace Irrigation.Application.Common.Sagas;

public interface ISagaHandler
{
    Task<SagaResult> Handle(object state, SagaContext context, CancellationToken ct = default);
}

public interface ISagaHandler<in TState> : ISagaHandler
{
    Task<SagaResult> Handle(TState state, SagaContext context, CancellationToken ct = default);

    Task<SagaResult> ISagaHandler.Handle(object state, SagaContext context, CancellationToken ct)
    {
        return Handle((TState)state, context, ct);
    }
}
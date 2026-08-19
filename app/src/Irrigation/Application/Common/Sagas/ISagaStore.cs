namespace Irrigation.Application.Common.Sagas;

public interface ISagaStore
{
    Task<Guid> Start<TState>(TState state, CancellationToken ct = default);
}
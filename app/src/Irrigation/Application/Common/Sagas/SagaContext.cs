namespace Irrigation.Application.Common.Sagas;

public sealed record SagaContext
{
    public Guid Id { get; init; }

    public int Attempts { get; init; }
}
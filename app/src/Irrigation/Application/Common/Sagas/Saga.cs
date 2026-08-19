namespace Irrigation.Application.Common.Sagas;

public static class Saga
{
    public static SagaResult Complete() => new SagaResult.Completed();

    public static SagaResult RetryAfter(TimeSpan after) => new SagaResult.Retry(DateTimeOffset.UtcNow.Add(after));

    public static SagaResult RetryAt(DateTimeOffset at) => new SagaResult.Retry(at);

    public static SagaResult Fail(string error) => new SagaResult.Failed(error);
}
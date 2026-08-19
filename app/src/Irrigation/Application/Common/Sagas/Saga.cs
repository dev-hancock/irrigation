namespace Irrigation.Application.Common.Sagas;

public static class Saga
{
    public static SagaResult Complete()
    {
        return new SagaResult.Completed();
    }

    public static SagaResult RetryAfter(TimeSpan after)
    {
        return new SagaResult.Retry(DateTimeOffset.UtcNow.Add(after));
    }

    public static SagaResult RetryAt(DateTimeOffset at)
    {
        return new SagaResult.Retry(at);
    }

    public static SagaResult Fail(string error)
    {
        return new SagaResult.Failed(error);
    }
}
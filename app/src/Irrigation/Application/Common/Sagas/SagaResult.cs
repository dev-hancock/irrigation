namespace Irrigation.Application.Common.Sagas
{
    public abstract record SagaResult
    {
        private SagaResult() { }

        public sealed record Completed : SagaResult;

        public sealed record Retry(DateTimeOffset At) : SagaResult;

        public sealed record Failed(string Error) : SagaResult;
    }
}

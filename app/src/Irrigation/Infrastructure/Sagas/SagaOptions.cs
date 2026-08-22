namespace Irrigation.Infrastructure.Sagas;

public sealed class SagaOptions     
{
    public const string Section = "Saga";

    public TimeSpan Retention { get; set; } = TimeSpan.FromDays(7);
}
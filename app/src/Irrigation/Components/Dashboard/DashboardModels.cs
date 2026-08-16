namespace Irrigation.Components.Dashboard;

public sealed record DashboardMetric(string Label, string Value, string Detail);

public sealed record ValveSummary(
    Guid Id,
    int Index,
    string Name,
    string Zone,
    string Status,
    DateTimeOffset UpdatedAt)
{
    public bool IsOpen => Status == "Open";

    private DateTimeOffset LocalUpdatedAt => UpdatedAt.ToLocalTime();

    public string UpdatedTime => LocalUpdatedAt.ToString("HH:mm:ss");

    public string UpdatedDate => LocalUpdatedAt.ToString("MMM d, yyyy");

    public string StatusSince => LocalUpdatedAt.ToString("MMM d, yyyy HH:mm:ss");

    public string LastUpdated => LocalUpdatedAt.ToString("MMM d, yyyy HH:mm:ss");
}

public sealed record ScheduleSummary(string Name, string Detail, bool IsRunning);

public sealed record ActivitySummary(string Time, string Description, string Source, bool IsSuccess);
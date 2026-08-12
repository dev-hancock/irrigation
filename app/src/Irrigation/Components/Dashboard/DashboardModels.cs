namespace Irrigation.Components.Dashboard;

public sealed record DashboardMetric(string Label, string Value, string Detail, string Icon, string Tone);

public sealed record ValveSummary(int Number, string Name, string Zone, bool IsOpen, string StatusSince, string LastUpdated);

public sealed record ScheduleSummary(string Name, string Detail, bool IsRunning);

public sealed record ActivitySummary(string Time, string Description, string Source, bool IsSuccess);
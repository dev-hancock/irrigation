using Irrigation.Domain.Shared;

namespace Irrigation.Application.Activities.Abstractions;

public interface IActivityFormatter
{
    ActivityType Type { get; }

    string[] GetArguments(string data);
}
using ErrorOr;

namespace Irrigation.Application.Extensions;

public static class ResultExtensions
{
    public static void ThrowIfError<T>(this ErrorOr<T> result)
    {
        if (!result.IsError)
        {
            return;
        }

        throw new ErrorOrException(result.Errors);
    }
}

public sealed class ErrorOrException(IReadOnlyList<Error> errors)
    : Exception(string.Join(", ", errors.Select(x => x.Description)))
{
    public IReadOnlyList<Error> Errors { get; } = errors;
}
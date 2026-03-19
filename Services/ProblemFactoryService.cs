using Microsoft.AspNetCore.Mvc;

namespace devhouse.Services;

public class ProblemFactoryService
{
    public static ProblemDetails BadRequest(string title, string details)
        => new ProblemDetails
        {
            Title = title,
            Detail = details,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        };

    public static ProblemDetails NotFound(int value)
        => new ProblemDetails
        {
            Title = "Not found",
            Detail = $"Unable to find resource with identifier: ({value})",
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found"
        };

    public static ProblemDetails Forbidden()
        => new ProblemDetails
        {
            Title = "Unauthorized",
            Detail = "You are lacking permissions to perform this action",
            Status = StatusCodes.Status403Forbidden,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden"
        };
}
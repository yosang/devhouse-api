using Microsoft.AspNetCore.Mvc;

namespace devhouse.Services;

/// <summary>Provides static methods that return ProblemDetails</summary>
public class ProblemResult
{
    /// <summary>Describes a mismatch in input id's</summary>
    /// <param name="param"></param>
    /// <param name="targetId"></param>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails IdMismatch(int param, int targetId)
        => new ProblemDetails
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({param}) Id and Request body Id ({targetId})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        };

    /// <summary>Describes a result of no match with provided search value</summary>
    /// <param name="value"></param>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails NoMatch(int value)
        => new ProblemDetails
        {
            Title = "Not found",
            Detail = $"Unable to find resource with identifier: ({value})",
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found"
        };

    /// <summary>Describes an attempt of an unauthorized operation</summary>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails NoPermissions()
        => new ProblemDetails
        {
            Title = "Unauthorized",
            Detail = "You are lacking permissions to perform this action",
            Status = StatusCodes.Status403Forbidden,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden"
        };

    /// <summary>Describes an attempt of an unauthenticated operation</summary>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails InvalidCredentials()
        => new ProblemDetails
        {
            Title = "Invalid credentials",
            Detail = "You are lacking valid authentication credentials",
            Status = StatusCodes.Status401Unauthorized,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-401-unauthorized"
        };
}
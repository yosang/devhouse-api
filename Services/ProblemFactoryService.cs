using Microsoft.AspNetCore.Mvc;

namespace devhouse.Services;

public class ProblemFactoryService
{
    /// <summary>Creates an instance of ProblemDetails describing a mismatch in input id's</summary>
    /// <param name="param"></param>
    /// <param name="targetId"></param>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails WhenIdMismatch(int param, int targetId)
        => new ProblemDetails
        {
            Title = "Id mismatch",
            Detail = $"There is a mismatch in the route path ({param}) Id and Request body Id ({targetId})",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        };

    /// <summary>Creates an instance of ProblemDetails describing failure of finding search value</summary>
    /// <param name="value"></param>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails WhenNotFound(int value)
        => new ProblemDetails
        {
            Title = "Not found",
            Detail = $"Unable to find resource with identifier: ({value})",
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found"
        };

    /// <summary>Creates an instance of ProblemDetails describing an attempt of an unauthorized operation</summary>
    /// <returns>class Microsoft.AspNetCore.Mvc.ProblemDetails</returns>
    public static ProblemDetails WhenForbidden()
        => new ProblemDetails
        {
            Title = "Unauthorized",
            Detail = "You are lacking permissions to perform this action",
            Status = StatusCodes.Status403Forbidden,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden"
        };
}
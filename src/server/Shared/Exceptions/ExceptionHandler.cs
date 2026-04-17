using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Genovel.Shared.Exceptions;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Exception: {Exception message}", exception.Message);

        // Identify which exception type to handle
        (string Detail, string Title, int Status) = exception switch
        {
            ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            _ => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        // Create problem details
        var problemDetails = new ProblemDetails()
        {
            Title = Title,
            Detail = Detail,
            Status = Status,
            Instance = context.Request.Path,
        };

        // Add extensions for problem details
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException ve)
        {
            problemDetails.Extensions.Add("ValidationErrors", ve.Errors);
        }

        // Write problem details into Response object
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}

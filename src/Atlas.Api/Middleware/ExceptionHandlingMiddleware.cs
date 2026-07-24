using Atlas.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var problemDetails = Map(exception);
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ProblemDetails Map(Exception exception) => exception switch
    {
        NotFoundException => new ProblemDetails
        {
            Title = "Resource not found.",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
        },
        ConflictException => new ProblemDetails
        {
            Title = "Conflict with the current state of the resource.",
            Detail = exception.Message,
            Status = StatusCodes.Status409Conflict,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
        },
        UnauthorizedException => new ProblemDetails
        {
            Title = "Authentication failed.",
            Detail = exception.Message,
            Status = StatusCodes.Status401Unauthorized,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        },
        _ => new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        }
    };
}

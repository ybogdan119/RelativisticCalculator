using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RelativisticCalculator.API.Exceptions;
using RelativisticCalculator.API.Models.Dto;

namespace RelativisticCalculator.API.Middleware;

/// <summary>
/// Middleware that handles all unhandled exceptions globally and returns standardized error responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <param name="logger">The logger instance used for logging exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware logic for handling exceptions.
    /// Catches specific known exceptions and returns appropriate HTTP responses.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (StarConflictException ex)
        {
            _logger.LogWarning(ex, "Conflict on star name(s)");

            var response = new InsertConflictDto
            {
                Message = ex.Message,
                StatusCode = 409,
                Conflicts = ex.ConflictingNames
            };

            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            await WriteErrorResponse(context, 404, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation");
            await WriteErrorResponse(context, 400, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponse(context, 500, "Internal server error", ex.Message);
        }
    }

    /// <summary>
    /// Writes a standardized error response to the client.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="message">The short error message.</param>
    /// <param name="details">Optional detailed description for debugging.</param>
    private static async Task WriteErrorResponse(HttpContext context, int statusCode, string message, string? details = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var error = new ErrorResponseDto
        {
            Message = message,
            Details = details,
            StatusCode = statusCode
        };

        await context.Response.WriteAsJsonAsync(error);
    }
}

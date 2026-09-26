using NexoDesk.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace NexoDesk.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, message) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                validationException.Errors.FirstOrDefault()?.ErrorMessage ?? "Dados inválidos."),
            ConflictException conflictException => (StatusCodes.Status409Conflict, conflictException.Message),
            ForbiddenException forbiddenException => (StatusCodes.Status403Forbidden, forbiddenException.Message),
            InvalidCredentialsException invalidCredentialsException => (StatusCodes.Status401Unauthorized, invalidCredentialsException.Message),
            UnauthorizedAccessException unauthorizedAccessException => (StatusCodes.Status401Unauthorized, unauthorizedAccessException.Message),
            KeyNotFoundException keyNotFoundException => (StatusCodes.Status404NotFound, keyNotFoundException.Message),
            InvalidOperationException invalidOperationException => (StatusCodes.Status409Conflict, invalidOperationException.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.")
        };

        if (status >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception while processing request.");
        }
        else
        {
            logger.LogWarning("Request failed with status code {Status}: {Message}", status, message);
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new ErrorResponse(status, message), cancellationToken);

        return true;
    }
}

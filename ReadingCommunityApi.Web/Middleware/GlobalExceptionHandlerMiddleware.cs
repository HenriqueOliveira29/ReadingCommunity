using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Exceptions;

namespace ReadingCommunityApi.Application.Middleware;
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        if (context.Response.HasStarted)
        {
            return Task.CompletedTask;
        }
        
        var response = exception switch
        {
            NotFoundException => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = exception.Message
            },
            ValidationException => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 400,
                Message = exception.Message
            },
            UnauthorizedException => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 401,
                Message = exception.Message
            },
            DbUpdateException => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "A database error occurred"
            },
            InvalidOperationException => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = exception.Message
            },
            _ => new OperationResult
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "An internal server error occurred"
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;
        return context.Response.WriteAsJsonAsync(response);
    }
}
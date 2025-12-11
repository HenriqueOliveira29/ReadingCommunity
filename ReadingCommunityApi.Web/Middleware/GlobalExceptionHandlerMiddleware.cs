using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
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
            EnrichSpanWithException(ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static void EnrichSpanWithException(Exception exception)
    {
        var activity = Activity.Current;
        
        if (activity == null)
            return;

        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
        
        activity.AddException(exception);
        
        activity.SetTag("error.type", exception.GetType().Name);
        activity.SetTag("error.handled", true);
        
        switch (exception)
        {
            case NotFoundException:
                activity.SetTag("error.category", "not_found");
                activity.SetTag("error.severity", "low");
                break;
            
            case ValidationException:
                activity.SetTag("error.category", "validation");
                activity.SetTag("error.severity", "low");
                break;
            
            case UnauthorizedException:
                activity.SetTag("error.category", "unauthorized");
                activity.SetTag("error.severity", "medium");
                break;
            
            case DbUpdateException:
                activity.SetTag("error.category", "database");
                activity.SetTag("error.severity", "high");
                break;
            
            case InvalidOperationException:
                activity.SetTag("error.category", "invalid_operation");
                activity.SetTag("error.severity", "medium");
                break;
            
            default:
                activity.SetTag("error.category", "unhandled");
                activity.SetTag("error.severity", "critical");
                break;
        }
        
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            activity.AddEvent(new ActivityEvent("exception.stacktrace",
                tags: new ActivityTagsCollection
                {
                    { "exception.stacktrace", exception.StackTrace }
                }));
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
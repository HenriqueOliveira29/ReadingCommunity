using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;

namespace ReadingCommunityApi.Application.Middleware;

public class RequestTelemetryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTelemetryMiddleware> _logger;
    private readonly ActivitySource _activitySource;
    private readonly Counter<long> _requestCounter;
    private readonly Histogram<double> _requestDuration;
    private readonly Histogram<long> _requestSize;
    private readonly Histogram<long> _responseSize;
    private readonly Counter<long> _activeRequests;

    public RequestTelemetryMiddleware(
        RequestDelegate next,
        ILogger<RequestTelemetryMiddleware> logger,
        ActivitySource activitySource,
        Meter meter)
    {
        _next = next;
        _logger = logger;
        _activitySource = activitySource;

        // Create metrics
        _requestCounter = meter.CreateCounter<long>(
            "http.server.requests",
            unit: "requests",
            description: "Total number of HTTP requests");

        _requestDuration = meter.CreateHistogram<double>(
            "http.server.request.duration",
            unit: "ms",
            description: "Duration of HTTP requests");

        _requestSize = meter.CreateHistogram<long>(
            "http.server.request.size",
            unit: "bytes",
            description: "Size of HTTP request bodies");

        _responseSize = meter.CreateHistogram<long>(
            "http.server.response.size",
            unit: "bytes",
            description: "Size of HTTP response bodies");

        _activeRequests = meter.CreateCounter<long>(
            "http.server.active_requests",
            unit: "requests",
            description: "Number of active HTTP requests");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;

        // Increment active requests
        var tags = CreateBasicTags(context);
        _activeRequests.Add(1, tags);

        var activity = Activity.Current;
        
        // Enrich with request information
        await EnrichWithRequestDataAsync(context, activity);

        // Capture the response
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            stopwatch.Stop();

            await EnrichWithResponseDataAsync(context, activity, responseBody);

            RecordMetrics(context, stopwatch.ElapsedMilliseconds, tags);

            LogRequest(context, stopwatch.ElapsedMilliseconds, startTime);

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            RecordErrorMetrics(context, stopwatch.ElapsedMilliseconds, ex, tags);

            LogError(context, stopwatch.ElapsedMilliseconds, startTime, ex);

            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            throw;
        }
        finally
        {
            // Decrement active requests
            _activeRequests.Add(-1, tags);

            // Copy the response back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task EnrichWithRequestDataAsync(HttpContext context, Activity? activity)
    {
        var request = context.Request;

        activity?.SetTag("http.method", request.Method);
        activity?.SetTag("http.scheme", request.Scheme);
        activity?.SetTag("http.target", request.Path + request.QueryString);
        activity?.SetTag("http.host", request.Host.ToString());
        activity?.SetTag("http.route", GetRoute(context));
        
        // Client information
        activity?.SetTag("http.client_ip", context.Connection.RemoteIpAddress?.ToString());
        activity?.SetTag("http.user_agent", request.Headers.UserAgent.ToString());
        
        activity?.SetTag("http.request_content_type", request.ContentType);
        activity?.SetTag("http.request_content_length", request.ContentLength ?? 0);
        
        // Headers
        if (request.Headers.ContainsKey("X-Request-ID"))
        {
            activity?.SetTag("http.request_id", request.Headers["X-Request-ID"].ToString());
        }
        
        // Query parameters count
        activity?.SetTag("http.query_params_count", request.Query.Count);
        
        // Authentication info
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            activity?.SetTag("user.authenticated", true);
            activity?.SetTag("user.name", context.User.Identity.Name);
            
            // Add user claims
            var userId = context.User.FindFirst("sub")?.Value 
                        ?? context.User.FindFirst("id")?.Value;
            if (userId != null)
            {
                activity?.SetTag("user.id", userId);
            }
        }
        else
        {
            activity?.SetTag("user.authenticated", false);
        }

        if (ShouldCaptureRequestBody(request))
        {
            await CaptureRequestBodyAsync(request, activity);
        }
    }

    private async Task EnrichWithResponseDataAsync(HttpContext context, Activity? activity, MemoryStream responseBody)
    {
        var response = context.Response;

        // Response information
        activity?.SetTag("http.status_code", response.StatusCode);
        activity?.SetTag("http.response_content_type", response.ContentType);
        activity?.SetTag("http.response_content_length", responseBody.Length);
        
        // Categorize the response
        var statusCategory = response.StatusCode switch
        {
            >= 200 and < 300 => "success",
            >= 300 and < 400 => "redirect",
            >= 400 and < 500 => "client_error",
            >= 500 => "server_error",
            _ => "unknown"
        };
        activity?.SetTag("http.status_category", statusCategory);

        if (response.StatusCode >= 400 && responseBody.Length > 0 && responseBody.Length < 10000)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(responseBody, Encoding.UTF8, leaveOpen: true);
            var responseText = await reader.ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            
            activity?.SetTag("http.response_body", responseText);
        }
    }

    private async Task CaptureRequestBodyAsync(HttpRequest request, Activity? activity)
    {
        // Only capture small request bodies
        if (request.ContentLength > 10000) // 10KB limit
        {
            activity?.SetTag("http.request_body", "[Body too large to capture]");
            return;
        }

        request.EnableBuffering();
        
        try
        {
            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);
            
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            
            if (!ContainsSensitiveData(body))
            {
                activity?.SetTag("http.request_body", body);
            }
            else
            {
                activity?.SetTag("http.request_body", "[Sensitive data redacted]");
            }
        }
        catch
        {
            request.Body.Position = 0;
        }
    }

    private void RecordMetrics(HttpContext context, double durationMs, KeyValuePair<string, object?>[] tags)
    {
        var request = context.Request;
        var response = context.Response;

        // Create extended tags for metrics
        var metricTags = tags.Concat(new[]
        {
            new KeyValuePair<string, object?>("http.status_code", response.StatusCode),
            new KeyValuePair<string, object?>("http.status_category", GetStatusCategory(response.StatusCode))
        }).ToArray();

        _requestCounter.Add(1, metricTags);

        _requestDuration.Record(durationMs, metricTags);

        if (request.ContentLength.HasValue)
        {
            _requestSize.Record(request.ContentLength.Value, metricTags);
        }

        _responseSize.Record(context.Response.Body.Length, metricTags);
    }

    private void RecordErrorMetrics(HttpContext context, double durationMs, Exception ex, KeyValuePair<string, object?>[] tags)
    {
        var errorTags = tags.Concat(new[]
        {
            new KeyValuePair<string, object?>("http.status_code", context.Response.StatusCode),
            new KeyValuePair<string, object?>("http.status_category", "server_error"),
            new KeyValuePair<string, object?>("exception.type", ex.GetType().Name)
        }).ToArray();

        _requestCounter.Add(1, errorTags);
        _requestDuration.Record(durationMs, errorTags);
    }

    private void LogRequest(HttpContext context, double durationMs, DateTime startTime)
    {
        var request = context.Request;
        var response = context.Response;

        _logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {Duration}ms | " +
            "ContentLength: {ContentLength} | ContentType: {ContentType} | " +
            "ClientIP: {ClientIP} | UserAgent: {UserAgent}",
            request.Method,
            request.Path + request.QueryString,
            response.StatusCode,
            durationMs,
            response.ContentLength ?? 0,
            response.ContentType ?? "unknown",
            context.Connection.RemoteIpAddress,
            request.Headers.UserAgent.ToString());
    }

    private void LogError(HttpContext context, double durationMs, DateTime startTime, Exception ex)
    {
        var request = context.Request;

        _logger.LogError(ex,
            "HTTP {Method} {Path} failed after {Duration}ms | " +
            "Exception: {ExceptionType} | Message: {Message} | " +
            "ClientIP: {ClientIP}",
            request.Method,
            request.Path + request.QueryString,
            durationMs,
            ex.GetType().Name,
            ex.Message,
            context.Connection.RemoteIpAddress);
    }

    private static KeyValuePair<string, object?>[] CreateBasicTags(HttpContext context)
    {
        return new[]
        {
            new KeyValuePair<string, object?>("http.method", context.Request.Method),
            new KeyValuePair<string, object?>("http.route", GetRoute(context)),
            new KeyValuePair<string, object?>("http.scheme", context.Request.Scheme)
        };
    }

    private static string GetRoute(HttpContext context)
    {
        // Try to get the route template
        var endpoint = context.GetEndpoint();
        if (endpoint is RouteEndpoint routeEndpoint)
        {
            return routeEndpoint.RoutePattern.RawText ?? context.Request.Path;
        }
        
        return context.Request.Path;
    }

    private static string GetStatusCategory(int statusCode)
    {
        return statusCode switch
        {
            >= 200 and < 300 => "success",
            >= 300 and < 400 => "redirect",
            >= 400 and < 500 => "client_error",
            >= 500 => "server_error",
            _ => "unknown"
        };
    }

    private static bool ShouldCaptureRequestBody(HttpRequest request)
    {
        // Only capture body for specific methods and content types
        if (request.Method != "POST" && request.Method != "PUT" && request.Method != "PATCH")
            return false;

        var contentType = request.ContentType?.ToLower() ?? "";
        return contentType.Contains("application/json") || 
               contentType.Contains("application/xml") ||
               contentType.Contains("text/");
    }

    private static bool ContainsSensitiveData(string body)
    {
        // Check for common sensitive field names
        var sensitiveFields = new[] 
        { 
            "password", "token", "secret", "apikey", "api_key", 
            "authorization", "credential", "ssn", "credit_card" 
        };
        
        var lowerBody = body.ToLower();
        return sensitiveFields.Any(field => lowerBody.Contains(field));
    }
}
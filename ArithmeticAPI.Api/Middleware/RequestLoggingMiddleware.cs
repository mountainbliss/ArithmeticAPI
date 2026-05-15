namespace ArithmeticAPI.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var start = DateTime.UtcNow;

        _logger.LogInformation(
            "Request started: {Method} {Path} at {Time}",
            context.Request.Method,
            context.Request.Path,
            start
        );

        try
        {
            await _next(context);

            var duration = DateTime.UtcNow - start;
            _logger.LogInformation(
                "Request finished: {Method} {Path} - {StatusCode} in {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                duration.TotalMilliseconds
            );
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - start;
            _logger.LogError(
                ex,
                "Request failed: {Method} {Path} - {ExceptionMessage} in {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                ex.Message,
                duration.TotalMilliseconds
            );

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            ArgumentException           => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException        => StatusCodes.Status404NotFound,
            DivideByZeroException       => StatusCodes.Status400BadRequest,
            _                           => StatusCodes.Status500InternalServerError
        };

        var errorResponse = new
        {
            error = ex.Message,
            statusCode = statusCode,
            timestamp = DateTime.UtcNow
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
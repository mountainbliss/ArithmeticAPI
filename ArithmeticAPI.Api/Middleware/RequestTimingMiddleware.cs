namespace ArithmeticAPI.Api.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    // Threshold in milliseconds — requests slower than this get a warning
    private const int SlowRequestThresholdMs = 500;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Stopwatch is more accurate than DateTime.UtcNow for measuring elapsed time
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        var method    = context.Request.Method;
        var path      = context.Request.Path;
        var status    = context.Response.StatusCode;

        // Log at different levels depending on how slow the request was
        if (elapsedMs > SlowRequestThresholdMs)
        {
            _logger.LogWarning(
                "[SLOW REQUEST] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms — exceeds {Threshold}ms threshold",
                method, path, status, elapsedMs, SlowRequestThresholdMs
            );
        }
        else
        {
            _logger.LogInformation(
                "[TIMING] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                method, path, status, elapsedMs
            );
        }
    }
}
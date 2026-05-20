namespace ArithmeticAPI.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    // The header name we use to pass the correlation ID
    // Using X- prefix is a convention for custom headers
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the caller already sent a correlation ID
        // This allows tracing across multiple services
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
                            ?? Guid.NewGuid().ToString();

        // Store it in HttpContext.Items so any code in this request can access it
        context.Items["CorrelationId"] = correlationId;

        // Add it to the response headers so the caller can see it
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        // Add it to the logging scope so it appears in every log message
        using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
        {
            _logger.LogInformation(
                "Request received with CorrelationId: {CorrelationId}",
                correlationId
            );

            await _next(context);
        }
    }
}
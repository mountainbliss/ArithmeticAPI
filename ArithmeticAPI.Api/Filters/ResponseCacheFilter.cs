using Microsoft.AspNetCore.Mvc.Filters;

namespace ArithmeticAPI.Api.Filters;

public class ResponseCacheFilter : IActionFilter
{
    private readonly int _durationSeconds;
    private readonly ILogger<ResponseCacheFilter> _logger;

    // Duration is passed in when the attribute is applied
    // allowing different endpoints to cache for different lengths of time
    public ResponseCacheFilter(int durationSeconds, ILogger<ResponseCacheFilter> logger)
    {
        _durationSeconds = durationSeconds;
        _logger          = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Nothing to do before the action runs
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Only cache successful responses
        if (context.HttpContext.Response.StatusCode == 200)
        {
            // Add cache control headers to the response
            context.HttpContext.Response.Headers.CacheControl =
                $"public, max-age={_durationSeconds}";

            context.HttpContext.Response.Headers.Vary =
                "Accept, Accept-Encoding";

            _logger.LogInformation(
                "[CACHE] Response cached for {Duration} seconds: {Path}",
                _durationSeconds,
                context.HttpContext.Request.Path
            );
        }
        else
        {
            // Never cache errors
            context.HttpContext.Response.Headers.CacheControl =
                "no-store";

            _logger.LogInformation(
                "[CACHE] Response not cached (status {StatusCode}): {Path}",
                context.HttpContext.Response.StatusCode,
                context.HttpContext.Request.Path
            );
        }
    }
}
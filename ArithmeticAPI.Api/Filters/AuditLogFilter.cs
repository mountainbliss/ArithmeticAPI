using Microsoft.AspNetCore.Mvc.Filters;

namespace ArithmeticAPI.Api.Filters;

public class AuditLogFilter : IActionFilter
{
    private readonly ILogger<AuditLogFilter> _logger;

    public AuditLogFilter(ILogger<AuditLogFilter> logger)
    {
        _logger = logger;
    }

    // Runs BEFORE the action method
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller    = context.RouteData.Values["controller"];
        var action        = context.RouteData.Values["action"];
        var arguments     = context.ActionArguments;
        var correlationId = context.HttpContext.Items["CorrelationId"] ?? "none";

        _logger.LogInformation(
            "[AUDIT] [{CorrelationId}] Executing: {Controller}/{Action} with arguments: {Arguments}",
            correlationId,
            controller,
            action,
            arguments
        );
    }

    // Runs AFTER the action method
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controller    = context.RouteData.Values["controller"];
        var action        = context.RouteData.Values["action"];
        var correlationId = context.HttpContext.Items["CorrelationId"] ?? "none";

        if (context.Exception != null)
        {
            _logger.LogError(
                "[AUDIT] [{CorrelationId}] Failed: {Controller}/{Action} — {Error}",
                correlationId,
                controller,
                action,
                context.Exception.Message
            );
        }
        else
        {
            _logger.LogInformation(
                "[AUDIT] [{CorrelationId}] Completed: {Controller}/{Action}",
                correlationId,
                controller,
                action
            );
        }
    }
}
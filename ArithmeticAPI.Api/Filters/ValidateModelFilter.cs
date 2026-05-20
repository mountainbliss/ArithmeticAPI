using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArithmeticAPI.Api.Filters;

public class ValidateModelFilter : IActionFilter
{
    private readonly ILogger<ValidateModelFilter> _logger;

    public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if model validation passed
        if (!context.ModelState.IsValid)
        {
            // Extract all validation errors into a readable format
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            _logger.LogWarning(
                "[VALIDATION] Model validation failed: {Errors}",
                System.Text.Json.JsonSerializer.Serialize(errors)
            );

            // Short-circuit — return 400 immediately
            // The action method never runs
            context.Result = new BadRequestObjectResult(new
            {
                message = "Validation failed",
                errors  = errors
            });
        }
    }

    // Nothing to do after the action
    public void OnActionExecuted(ActionExecutedContext context) { }
}
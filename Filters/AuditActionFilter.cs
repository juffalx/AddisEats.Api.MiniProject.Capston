using Microsoft.AspNetCore.Mvc.Filters;

namespace AddisEats.Api.MiniProject.Capston.Filters;

public class AuditActionFilter : IActionFilter
{
    private readonly ILogger<AuditActionFilter> _logger;

    public AuditActionFilter(ILogger<AuditActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        var arguments = context.ActionArguments;

        _logger.LogInformation(
            "Action {ActionName} called with arguments: {@Arguments}",
            actionName,
            arguments);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
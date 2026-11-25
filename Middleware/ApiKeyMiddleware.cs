using AddisEats.Api.MiniProject.Capston.Services.Test;

namespace AddisEats.Api.MiniProject.Capston.Middleware;

public class ApiKeyMiddleware
{
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private readonly RequestDelegate _next;
    private  TestScopedService _testService;


    public ApiKeyMiddleware(
        ILogger<ApiKeyMiddleware> logger,
        RequestDelegate next
    )
    {
        _logger = logger;
        _next = next;

    }

    public async Task InvokeAsync(HttpContext context, TestScopedService testService
)
    {
        _testService = testService;

        // "X-Api-Key የkey:value የ key ስሙ ነው እና

        var api = context.Request.Headers["X-Api-Key"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(api))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);

        _logger.LogInformation("{Message}", _testService.GetMessage());
    }
}
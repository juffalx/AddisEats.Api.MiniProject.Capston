namespace AddisEats.Api.MiniProject.Capston.Middleware;

public class ApiKeyMiddleware
{
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(
        ILogger<ApiKeyMiddleware> logger,
        RequestDelegate next
    )
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // "X-Api-Key

        var api = context.Request.Headers["X-Api-Key"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(api))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}
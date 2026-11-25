namespace AddisEats.Api.MiniProject.Capston.Middleware;

public class CorrelationIdMiddleware
{
    // የCorrelationId di inject እናደርገዋለን ፤ log እና next እንደ ፓራሜትር ተጠቅመን
    private readonly ILogger<CorrelationIdMiddleware> _log;
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> log, RequestDelegate next)
    {
        _log = log;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1, id check እናደርግ እና ከሌለ null ካለ ደግሞ value  id ላይ store እናደርገዋለን
        var id = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();

        // 2, ከሌለ አዲስ id create እናደርጋለን
        if (string.IsNullOrWhiteSpace(id))
        {
            id = Guid.NewGuid().ToString("N");
        }
        context.TraceIdentifier = id;
        context.Response.Headers["X-Correlation-Id"] = id;

        await _next(context);

        _log.LogInformation(
            "Method {Method} CorrelationId: {CorrelationId}",
            context.Request.Method,
            id);
    }
}
using System.Diagnostics;

namespace AddisEats.Api.MiniProject.Capston.Middleware;

public class RequestTimingMiddleware
{
    // di inject እናደርጋለን
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // timer እናስጀምረው እና  next እናደጋለን
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            // response ሲመጣ እንቆመው እና ከዛ ቡሃላ በ ሚሊ ሰከንድ ቀይረን እናስቀምጠዋለን
            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var method = context.Request.Method;
            var path = context.Request.Path;
            var status = context.Response.StatusCode;

            // ከ ሰከንድ በላይ ከቆየ logWarning እና ተቃራኒ ከሆነ ደግሞ logInformation እናደርጋለን
            if (elapsedMilliseconds > 1000)
            {
                _logger.LogWarning(
                    "Request {Method} {Path} Status {Status} executed in {ElapsedMilliseconds} ms",
                    method,
                    path,
                    status,
                    elapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "Request {Method} {Path} Status {Status} executed in {ElapsedMilliseconds} ms",
                    method,
                    path,
                    status,
                    elapsedMilliseconds);
            }
        }
    }
}
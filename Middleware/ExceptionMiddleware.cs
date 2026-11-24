using System.Net;  // http ሳታተስ ኮድ ይሰጠናል ለምሳሌ ያክል እንድ 500 እና another
using System.Text.Json; // c# object ወደ json እንድንቀይረው ያደርገናል

namespace AddisEats.Api.MiniProject.Capston.Middleware;

public class ExceptionMiddleware
{
    //  unexpected errors  API ካለ ካች ማድረግ ነው.
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "An unexpected server error occurred."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
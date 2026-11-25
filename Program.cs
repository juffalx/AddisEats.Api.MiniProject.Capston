
using AddisEats.Api.MiniProject.Capston.Middleware;
using AddisEats.Api.MiniProject.Capston.Services.OrderService;
using AddisEats.Api.MiniProject.Capston.Services.RestaurantService;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;

        o.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;

        o.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });
var app = builder.Build();


// app.MapGet("/test-error", () =>
// {
//     throw new Exception("Test exception");
// });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseCors("frontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();




app.Run();

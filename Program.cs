var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// GET /getWelcome?name=yourName
// Returns JSON with a welcome message: { "message": "<name> welcome back" }
app.MapGet("/getWelcome", (Microsoft.AspNetCore.Http.HttpRequest req) =>
{
    var name = req.Query["name"].ToString();
    if (string.IsNullOrWhiteSpace(name))
    {
        return Results.BadRequest(new { error = "Query parameter 'name' is required." });
    }
    return Results.Ok(new { message = WelcomeService.CreateMessage(name) });
})
.WithName("getWelcome");

// POST /getWelcome
// Accepts JSON { "name": "..." } and returns { "message": "<name> welcome back" }
app.MapPost("/getWelcome", async (Microsoft.AspNetCore.Http.HttpRequest req) =>
{
    var body = await req.ReadFromJsonAsync<Dictionary<string, string?>>();
    if (body == null || !body.TryGetValue("name", out var name) || string.IsNullOrWhiteSpace(name))
    {
        return Results.BadRequest(new { error = "JSON body must contain 'name'." });
    }
    return Results.Ok(new { message = WelcomeService.CreateMessage(name) });
})
.WithName("postGetWelcome");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class WelcomeService
{
    public static string CreateMessage(string name) => name + " welcome back";
}

// Expose Program class for WebApplicationFactory in integration tests
public partial class Program { }

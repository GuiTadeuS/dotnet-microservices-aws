using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Precipitation.Extensions;
using Weather.Temperature.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/observation/{ddd}", async (string ddd, [FromQuery] int? days, TemperatureDbContext context) =>
{
    if (days == null || days < 1 || days > 30) return Results
                                                .BadRequest("No days provided or days not between 1 and 30");

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = await context.Temperatures
                    .Where(p => p.Ddd == ddd && p.CreatedOn >= startDate)
                    .ToListAsync();

    return Results.Ok(results);

});

app.MapPost("/observation", async ([FromBody] Temperature temperature, TemperatureDbContext context) =>
{
    temperature.CreatedOn = temperature.CreatedOn.ToUniversalTime();
    await context.AddAsync(temperature);
    await context.SaveChangesAsync();
});

app.Run();
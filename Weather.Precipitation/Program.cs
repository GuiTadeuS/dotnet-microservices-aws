using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Precipitation.DataAccess;
using Weather.Precipitation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/observation/{cep}", async (string cep, [FromQuery] int? days, PrecipDbContext context) =>
{
    if (days == null || days < 1 || days > 30) return Results
                                                .BadRequest("No days provided or days not between 1 and 30");

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = await context.Precipitations
                    .Where(p => p.Cep == cep && p.CreatedOn >= startDate )
                    .ToListAsync();

    return Results.Ok(results);
    
});

app.Run();
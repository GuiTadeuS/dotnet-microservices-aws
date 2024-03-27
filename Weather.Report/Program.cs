using Microsoft.AspNetCore.Mvc;
using Weather.Precipitation.Extensions;
using Weather.Report.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/weather-report/{ddd}", async (string ddd, [FromQuery] int? days, IWeatherReportAggregator weatherAgg) =>
{
    if (days == null) return Results.BadRequest("\"No days provided or days not between 1 and 30");

    var report = await weatherAgg.BuildWeeklyReport(ddd, days.Value);

    return Results.Ok(report);
});
app.Run();
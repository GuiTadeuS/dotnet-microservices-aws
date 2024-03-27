using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Weather.DataLoader.Models;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var servicesConfig = config.GetSection("Services");

var temperatureServiceConfig = servicesConfig.GetSection("Temperature");
var temperatureServiceHost = temperatureServiceConfig["Host"];
var temperatureServicePort = temperatureServiceConfig["Port"];

var precipitationServiceConfig = servicesConfig.GetSection("Precipitation");
var precipitationServiceHost = precipitationServiceConfig["Host"];
var precipitationServicePort = precipitationServiceConfig["Port"];

var dddCodes = new List<string>
{
    "01234",
    "56789",
    "12345",
    "67890",
    "23456",
};

Console.WriteLine("Starting Data Load");

var temperatureHttpClient = new HttpClient();
temperatureHttpClient.BaseAddress = new Uri($"http://{temperatureServiceHost}:{temperatureServicePort}");

var precipitationHttpClient = new HttpClient();
precipitationHttpClient.BaseAddress = new Uri($"http://{precipitationServiceHost}:{precipitationServicePort}");

foreach (var ddd in dddCodes)
{
    Console.WriteLine($"Loading data for DDD {ddd}");

    var from = DateTime.Now.AddDays(-2);
    var to = DateTime.Now;

    for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
    {
        var temperature = PostTemp(ddd, day, temperatureHttpClient);
        PostPrecipitation(temperature[0], ddd, day, precipitationHttpClient);
    }
}

void PostPrecipitation(int lowTemp ,string ddd, DateTime day, HttpClient precipitationHttpClient)
{
    var rand = new Random();

    var isPrecip = rand.Next(2) < 1;

    PrecipitationModel precipitation = new PrecipitationModel
    {
        Ddd = ddd,
        CreatedOn = day,
        Millimeters = 0,
        WeatherType = "none"
    };

    if(isPrecip)
    {
        var precipMillimeters = rand.Next(1, 16);
        if (lowTemp < 18)
        {
            precipitation.Millimeters = precipMillimeters;
            precipitation.WeatherType = "rain";
        }
    }

    var precipResponse = precipitationHttpClient
        .PostAsJsonAsync("observation", precipitation)
        .Result;

    if(precipResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Precipitation data for DDD {ddd} on {day} loaded successfully");
    }
       else
    {
        Console.WriteLine(precipResponse.ToString());
    }
}


List<int> PostTemp(string ddd, DateTime day, HttpClient httpClient)
{
    var rand = new Random();
    var t1 = rand.Next(0, 36);
    var t2 = rand.Next(0, 36);

    var hiLoTemps = new List<int> { t1, t2 };
    hiLoTemps.Sort();

    var temperatureObservation = new TemperatureModel
    {
        Ddd = ddd,
        CreatedOn = day,
        TempLowC = hiLoTemps[0],
        TempHighC = hiLoTemps[1]
    };

    var tempResponse = httpClient
        .PostAsJsonAsync("observation", temperatureObservation)
        .Result;

    if(tempResponse.IsSuccessStatusCode)
    {         
        Console.WriteLine($"Temperature data for DDD {ddd} on {day} loaded successfully");
    }
    else
    {
        Console.WriteLine(tempResponse.ToString());
    }

    return hiLoTemps;
}
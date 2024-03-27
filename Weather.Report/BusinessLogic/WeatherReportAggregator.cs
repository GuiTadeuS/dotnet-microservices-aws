using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Weather.Report.Config;
using Weather.Report.DataAccess;
using Weather.Report.Models;

namespace Weather.Report.BusinessLogic
{
    public interface IWeatherReportAggregator
    {
        Task<WeatherReport> BuildWeeklyReport(string ddd, int days);
    }
    
    public class WeatherReportAggregator : IWeatherReportAggregator
    {
        private readonly IHttpClientFactory _http;
        private readonly ILogger<WeatherReportAggregator> _logger;
        private readonly WeatherDataConfig _weatherDataConfig;
        private readonly WeatherReportDbContext _context;

        public WeatherReportAggregator(
            IHttpClientFactory http,
            ILogger<WeatherReportAggregator> logger,
            IOptions<WeatherDataConfig> weatherDataConfig,
            WeatherReportDbContext context
        ){
            _http = http;
            _logger = logger;
            _weatherDataConfig = weatherDataConfig.Value;
            _context = context;
        }

        public async Task<WeatherReport> BuildWeeklyReport(string ddd, int days)
        {
            var httpClient = _http.CreateClient();

            var precipData = await FetchPrecipitationData(httpClient, ddd, days);
            var totalRain = GetTotalRain(precipData);
            _logger.LogInformation($"DDD: {ddd} - Days: {days} - Rain: {totalRain}");

            var temperatureData = await FetchTemperatureData(httpClient, ddd, days);
            var averageHigh = temperatureData.Average(t => t.TempHighC);
            var averageLow = temperatureData.Average(t => t.TempLowC);
            _logger.LogInformation($"DDD: {ddd} - Days: {days} - Avg High: {averageHigh} - Avg Low: {averageLow}");

            var report = new WeatherReport
            {
                AverageHighC = Math.Round(averageHigh, 1),
                AverageLowC = Math.Round(averageLow, 1),
                RainfallTotalMillimeters = totalRain,
                Ddd = ddd,
                CreatedOn = DateTime.UtcNow
            };

            _context.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }

        private decimal GetTotalRain(IEnumerable<PrecipitationModel> precipData)
        {
            var totalRain = precipData
                .Where(type => type.WeatherType == "rain")
                .Sum(m => m.Millimeters);
            var result = Math.Round(totalRain, 1);
            return result;
        }

        private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string ddd, int days)
        {
            var endpoint = BuildTemperatureServiceEndpoint(ddd, days);
            var temperatureRecords = await httpClient.GetAsync(endpoint);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var temperatureData = await temperatureRecords
                .Content
                .ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializerOptions);
            return temperatureData ?? new List<TemperatureModel>(); // If null, return empty list
        }

        private async Task<List<PrecipitationModel>> FetchPrecipitationData(HttpClient httpClient, string ddd, int days)
        {
            var endpoint = BuildPrecipitationServiceEndpoint(ddd, days);
            var precipitationRecords = await httpClient.GetAsync(endpoint);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var precipitationData = await precipitationRecords
                .Content
                .ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializerOptions);
            return precipitationData ?? new List<PrecipitationModel>(); 
        }

        private string? BuildTemperatureServiceEndpoint(string ddd, int days)
        {
            var tempServiceProtocol = _weatherDataConfig.TempDataProtocol;
            var tempServiceHost = _weatherDataConfig.TempDataHost;
            var tempServicePort = _weatherDataConfig.TempDataPort;
            return $"{tempServiceProtocol}://{tempServiceHost}:{tempServicePort}/observation/{ddd}?days={days}";
        }

        private string? BuildPrecipitationServiceEndpoint(string ddd, int days)
        {
            var precipServiceProtocol = _weatherDataConfig.PrecipDataProtocol;
            var precipServiceHost = _weatherDataConfig.PrecipDataHost;
            var precipServicePort = _weatherDataConfig.PrecipDataPort;
            return $"{precipServiceProtocol}://{precipServiceHost}:{precipServicePort}/observation/{ddd}?days={days}";
        }
    }
}

namespace Weather.Precipitation.DataAccess
{
    public class Precipitation
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Millimeters { get; set; }
        public string? WeatherType { get; set; }
        public string? Ddd { get; set; }
    }
}

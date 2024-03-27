namespace Weather.Report.DataAccess
{
    public class WeatherReport
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal AverageHighC { get; set; }
        public decimal AverageLowC { get; set; }
        public decimal RainfallTotalMillimeters { get; set; }
        public string? Ddd { get; set; }
    }
}

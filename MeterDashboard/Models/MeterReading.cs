
namespace MeterDashboard.Models
{
    public class MeterReading
    {
        public DateTime Timestamp { get; set; }
        public string? MeterId { get; set; }
        public double Value { get; set; }
    }
}
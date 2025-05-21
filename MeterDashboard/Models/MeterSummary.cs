
namespace MeterDashboard.Models
{
    public class MeterSummary
    {
        public string? Period { get; set; } // e.g., date, week, or month label
        public double Sum { get; set; }
        public double PeakValue { get; set; }
        public DateTime PeakTime { get; set; }
    }
}
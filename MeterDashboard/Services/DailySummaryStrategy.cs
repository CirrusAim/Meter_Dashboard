
namespace MeterDashboard.Services
{
    public class DailySummaryStrategy : ISummaryStrategy
    {
        public string GetPeriodFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public string GetPeriodLabel(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public DateTime GetNextPeriod(DateTime date)
        {
            return date.Date.AddDays(1); // Include 00:00 AM of next day
        }

        public bool IsInPeriod(DateTime date, DateTime periodStart)
        {
            var nextDay = GetNextPeriod(periodStart);
            return (date >= periodStart && date < nextDay) ||
                   (date == nextDay); // Ensuring that Period for the day is between Current day 01:00 - 23:59 to the Next day 00:00
        }

        public DateTime GetPeriodStart(DateTime date)
        {
            return date.Date.AddHours(1); // Start a new day reading from (01:00 AM)
        }
    }
}

using static System.Net.Mime.MediaTypeNames;

namespace MeterDashboard.Services
{
    public class MonthlySummaryStrategy : ISummaryStrategy
    {
        public string GetPeriodFormat(DateTime date)
        {
            return $"{date.Year}-{date.Month:D2}";
        }

        public string GetPeriodLabel(DateTime date)
        {
            return $"{date.Year}-{date.Month:D2}";
        }

        public DateTime GetNextPeriod(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1); // Include 00:00 AM of first day of next month
        }

        public bool IsInPeriod(DateTime date, DateTime periodStart)
        {
            var nextMonth = new DateTime(periodStart.Year, periodStart.Month, 1).AddMonths(1);
            return (date >= periodStart && date < nextMonth) ||
                   (date == nextMonth); // Ensuring that Period for the month is between Current month 01:00 - 23:59 to the Next month 00:00
        }

        public DateTime GetPeriodStart(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddHours(1); // Start a new Month reading from (01:00 AM)
        }
    }
}
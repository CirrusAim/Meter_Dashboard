using System.Globalization;

namespace MeterDashboard.Services
{
    public class WeeklySummaryStrategy : ISummaryStrategy
    {
        public string GetPeriodFormat(DateTime date)
        {
            return $"{date.Year}-W{ISOWeek.GetWeekOfYear(date):D2}";
        }

        public string GetPeriodLabel(DateTime date)
        {
            return $"{date.Year}-W{ISOWeek.GetWeekOfYear(date):D2}";
        }

        public DateTime GetNextPeriod(DateTime date)
        {
            // Get the Monday of the week
            var dayOfWeek = (int)date.DayOfWeek;
            var diff = (dayOfWeek + 6) % 7;
            var monday = date.Date.AddDays(-diff);
            return monday.AddDays(7); // Include 00:00 AM of next Monday to previous week
        }

        public bool IsInPeriod(DateTime date, DateTime periodStart)
        {
            var nextWeek = GetNextPeriod(periodStart);
            return (date >= periodStart && date < nextWeek) ||
                   (date == nextWeek); // Ensuring that Period for the week is between Current week Monday, 01:00-23:59 to Next week Monday, 00:00
        }

        public DateTime GetPeriodStart(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var diff = (dayOfWeek + 6) % 7;
            var monday = date.Date.AddDays(-diff);
            return monday.AddHours(1);   // Start a new Week reading from (01:00 AM)

        }
    }
}
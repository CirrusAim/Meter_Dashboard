using System.Globalization;

namespace MeterDashboard.Services
{
    public class QuarterlySummaryStrategy : ISummaryStrategy
    {
        public string GetPeriodFormat(DateTime date)
        {
            var quarter = (date.Month - 1) / 3 + 1;
            return $"{date.Year} - Q{quarter}";
        }

        public string GetPeriodLabel(DateTime date)
        {
            var quarter = (date.Month - 1) / 3 + 1;
            return $"{date.Year} - Q{quarter}";
        }

        public DateTime GetNextPeriod(DateTime date)
        {
            // Get the Monday of the week
            var quarter = (date.Month - 1) / 3 + 1;
            var nextQuarterMonth = quarter * 3 + 1;
            return new DateTime(date.Year, nextQuarterMonth, 1);
        }

        public bool IsInPeriod(DateTime date, DateTime periodStart)
        {
            var nextQuarter = GetNextPeriod(periodStart);
            return (date >= periodStart && date < nextQuarter) || (date == periodStart);

        }

        public DateTime GetPeriodStart(DateTime date)
        {
            var quarter = (date.Month - 1) / 3 + 1;
            var quarterStartMonth = (quarter - 1) * 3 + 1;
            return new DateTime(date.Year, quarterStartMonth, 1).AddHours(1);
        }
    }
}
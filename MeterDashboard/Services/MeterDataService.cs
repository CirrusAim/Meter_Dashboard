
using System.Globalization;
using System.Text;
using MeterDashboard.Models;


namespace MeterDashboard.Services
{
    public class MeterDataService : IMeterDataService
    {
        private List<MeterReading> readings = new();
        private bool loaded = false;
        protected virtual string dataPath => "wwwroot/data/data.csv";
        private readonly ISummaryStrategy _dailyStrategy;
        private readonly ISummaryStrategy _weeklyStrategy;
        private readonly ISummaryStrategy _monthlyStrategy;

        public MeterDataService()
        {
            _dailyStrategy = new DailySummaryStrategy();
            _weeklyStrategy = new WeeklySummaryStrategy();
            _monthlyStrategy = new MonthlySummaryStrategy();
        }

        public async Task LoadAsync()
        {
            if (loaded) return;
            if (!File.Exists(dataPath)) throw new FileNotFoundException($"CSV not found: {dataPath}");
            var lines = await File.ReadAllLinesAsync(dataPath, Encoding.UTF8);
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length != 3) continue;
                if (!long.TryParse(parts[0], out var unix)) continue;
                if (!double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var value)) continue;
                readings.Add(new MeterReading
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(unix).DateTime,
                    MeterId = parts[1],
                    Value = value
                });
            }
            loaded = true;
        }

        public List<string> GetMeterIds() =>
            readings.Select(r => r.MeterId ?? string.Empty)
                   .Distinct()
                   .Where(id => !string.IsNullOrEmpty(id))
                   .ToList();

        public List<int> GetYears(string meterId)
        {
            return readings.Where(r => r.MeterId == meterId)
                .Select(r => r.Timestamp.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();
        }

        private List<MeterSummary> GetSummary(string meterId, int year, ISummaryStrategy strategy)
        {
            var result = new List<MeterSummary>();

            var meterReadings = readings
                .Where(r => r.MeterId == meterId && r.Timestamp.Year == year)
                .OrderBy(r => r.Timestamp)
                .ToList();

            // Get all unique period starts for the year
            var periodStarts = meterReadings
                .Select(r => strategy.GetPeriodStart(r.Timestamp))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            foreach (var periodStart in periodStarts)
            {
                // Get readings using the strategy's "IsInPeriod" method
                var periodReadings = meterReadings
                    .Where(r => strategy.IsInPeriod(r.Timestamp, periodStart))
                    .ToList();

                if (periodReadings.Any())
                {
                    var peak = periodReadings.OrderByDescending(x => x.Value).First();
                    result.Add(new MeterSummary
                    {
                        Period = strategy.GetPeriodLabel(periodStart),
                        Sum = periodReadings.Sum(r => r.Value),
                        PeakValue = peak.Value,
                        PeakTime = peak.Timestamp
                    });
                }
            }

            return result;
        }

        public List<MeterSummary> GetDailySummary(string meterId, int year)
        {
            return GetSummary(meterId, year, _dailyStrategy);
        }

        public List<MeterSummary> GetWeeklySummary(string meterId, int year)
        {
            return GetSummary(meterId, year, _weeklyStrategy);
        }

        public List<MeterSummary> GetMonthlySummary(string meterId, int year)
        {
            return GetSummary(meterId, year, _monthlyStrategy);
        }
    }
}
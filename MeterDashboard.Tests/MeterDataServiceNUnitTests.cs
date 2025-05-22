
using MeterDashboard.Services;
using System.Globalization;

namespace MeterDashboard.Tests
{
    [TestFixture]
    public class DailySummaryStrategyTests
    {
        private DailySummaryStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new DailySummaryStrategy();
        }

        [Test]
        public void GetPeriodStart_ReturnsCorrectStartTime()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var start = _strategy.GetPeriodStart(date);
            Assert.That(start, Is.EqualTo(new DateTime(2011, 6, 15, 1, 0, 0)));
        }

        [Test]
        public void IsInPeriod_IncludesNextDayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var nextDayMidnight = new DateTime(2011, 6, 16, 0, 0, 0);
            Assert.That(_strategy.IsInPeriod(nextDayMidnight, date), Is.True);
        }

        [Test]
        public void GetNextPeriod_ReturnsNextDayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var next = _strategy.GetNextPeriod(date);
            Assert.That(next, Is.EqualTo(new DateTime(2011, 6, 16, 0, 0, 0)));
        }

        [Test]
        public void GetPeriodFormat_ReturnsCorrectFormat()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var format = _strategy.GetPeriodFormat(date);
            Assert.That(format, Is.EqualTo("2011-06-15"));
        }

        [Test]
        public void GetPeriodLabel_ReturnsCorrectLabel()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var label = _strategy.GetPeriodLabel(date);
            Assert.That(label, Is.EqualTo("2011-06-15"));
        }
    }

    [TestFixture]
    public class WeeklySummaryStrategyTests
    {
        private WeeklySummaryStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new WeeklySummaryStrategy();
        }

        [Test]
        public void GetPeriodStart_ReturnsMondayOneAM()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0); // Wednesday
            var start = _strategy.GetPeriodStart(date);
            Assert.That(start, Is.EqualTo(new DateTime(2011, 6, 13, 1, 0, 0))); // Monday 1 AM
        }

        [Test]
        public void IsInPeriod_IncludesNextMondayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0); // Wednesday
            var nextMondayMidnight = new DateTime(2011, 6, 20, 0, 0, 0); // Next Monday
            Assert.That(_strategy.IsInPeriod(nextMondayMidnight, date), Is.True);
        }

        [Test]
        public void GetNextPeriod_ReturnsNextMondayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0); // Wednesday
            var next = _strategy.GetNextPeriod(date);
            Assert.That(next, Is.EqualTo(new DateTime(2011, 6, 20, 0, 0, 0))); // Next Monday
        }

        [Test]
        public void GetPeriodFormat_ReturnsCorrectFormat()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0); // Wednesday
            var format = _strategy.GetPeriodFormat(date);
            Assert.That(format, Is.EqualTo("2011-W24"));
        }

        [Test]
        public void GetPeriodLabel_ReturnsCorrectLabel()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0); // Wednesday
            var label = _strategy.GetPeriodLabel(date);
            Assert.That(label, Is.EqualTo("2011-W24"));
        }
    }

    [TestFixture]
    public class MonthlySummaryStrategyTests
    {
        private MonthlySummaryStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new MonthlySummaryStrategy();
        }

        [Test]
        public void GetPeriodStart_ReturnsFirstDayOneAM()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var start = _strategy.GetPeriodStart(date);
            Assert.That(start, Is.EqualTo(new DateTime(2011, 6, 1, 1, 0, 0)));
        }

        [Test]
        public void IsInPeriod_IncludesNextMonthFirstDayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var nextMonthFirstDayMidnight = new DateTime(2011, 7, 1, 0, 0, 0);
            Assert.That(_strategy.IsInPeriod(nextMonthFirstDayMidnight, date), Is.True);
        }

        [Test]
        public void GetNextPeriod_ReturnsNextMonthFirstDayMidnight()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var next = _strategy.GetNextPeriod(date);
            Assert.That(next, Is.EqualTo(new DateTime(2011, 7, 1, 0, 0, 0)));
        }

        [Test]
        public void GetPeriodFormat_ReturnsCorrectFormat()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var format = _strategy.GetPeriodFormat(date);
            Assert.That(format, Is.EqualTo("2011-06"));
        }

        [Test]
        public void GetPeriodLabel_ReturnsCorrectLabel()
        {
            var date = new DateTime(2011, 6, 15, 12, 0, 0);
            var label = _strategy.GetPeriodLabel(date);
            Assert.That(label, Is.EqualTo("2011-06"));
        }
    }

    [TestFixture]
    public class MeterDataServiceNUnitTests
    {
        private MeterDataService _service;
        private string _dataPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dataPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "data.csv");

            if (!File.Exists(_dataPath))
            {
                throw new FileNotFoundException($"Required test data file not found: {_dataPath}");
            }
        }

        [SetUp]
        public async Task SetUp()
        {
            _service = new TestMeterDataService(_dataPath);
            await _service.LoadAsync();
        }

        [Test]
        public void Test_GetMeterIds_Returns_MeterIds()
        {
            var ids = _service.GetMeterIds();
            Assert.That(ids, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Test_GetYears_Returns_MeterIdYears()
        {
            var meterId = _service.GetMeterIds().First();
            var years = _service.GetYears(meterId);
            Assert.That(years, Is.Not.Null.And.Not.Empty);
            Assert.That(years, Does.Contain(2011));
        }

        [Test]
        public void GetDailySummary_Returns_Summary_For_MeterId_And_Year()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetDailySummary(meterId, 2011);
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetWeeklySummary_Returns_Summary_For_MeterId_And_Year()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetWeeklySummary(meterId, 2011);
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetMonthlySummary_Returns_Summary_For_MeterId_And_Year()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetMonthlySummary(meterId, 2011);
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Test_LoadAsync_For_DataSource()
        {
            var service = new TestMeterDataService("nonexistent.csv");
            Assert.ThrowsAsync<FileNotFoundException>(async () => await service.LoadAsync());
        }

        // Edge Cases Tests ---------------------------------------------------------------------
        [Test]
        public void GetDailySummary_HandlesMonthTransition()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetDailySummary(meterId, 2011);

            // Check if the last day of June includes the 00:00 reading of July
            var lastDayOfJune = new DateTime(2011, 6, 30);
            Assert.That(summary.Any(s => s.Period == lastDayOfJune.ToString("yyyy-MM-dd")), Is.True);
        }

        [Test]
        public void GetWeeklySummary_HandlesMonthTransition()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetWeeklySummary(meterId, 2011);

            // Check if the last week of June includes the 00:00 reading of July's first week
            var dateInLastWeekOfJune = new DateTime(2011, 6, 27); // A date in the last week of June
            var weeklyStrategy = new WeeklySummaryStrategy();
            var expectedPeriod = weeklyStrategy.GetPeriodLabel(dateInLastWeekOfJune);
            Assert.That(summary.Any(s => s.Period == expectedPeriod), Is.True);
        }

        [Test]
        public void GetMonthlySummary_HandlesMonthTransition()
        {
            var meterId = _service.GetMeterIds().First();
            var summary = _service.GetMonthlySummary(meterId, 2011);

            // Check if June includes the 00:00 reading of July
            var dateInJune = new DateTime(2011, 6, 15);
            var monthlyStrategy = new MonthlySummaryStrategy();
            var expectedPeriod = monthlyStrategy.GetPeriodLabel(dateInJune);
            Assert.That(summary.Any(s => s.Period == expectedPeriod), Is.True);
        }

        private int GetIsoWeekOfYear(DateTime date)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private class TestMeterDataService : MeterDataService
        {
            private readonly string _dataPath;

            public TestMeterDataService(string dataPath)
            {
                _dataPath = dataPath;
            }

            protected override string dataPath => _dataPath;
        }
    }
}

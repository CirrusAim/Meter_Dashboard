
namespace MeterDashboard.Services
{
    public interface ISummaryStrategy
    {
        string GetPeriodFormat(DateTime date);
        string GetPeriodLabel(DateTime date);
        DateTime GetNextPeriod(DateTime date);
        bool IsInPeriod(DateTime date, DateTime periodStart);
        DateTime GetPeriodStart(DateTime date);
    }
}
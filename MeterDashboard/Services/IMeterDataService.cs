using MeterDashboard.Models;

namespace MeterDashboard.Services
{
    public interface IMeterDataService
    {
        Task LoadAsync();
        List<string> GetMeterIds();
        List<int> GetYears(string meterId);
        List<MeterSummary> GetDailySummary(string meterId, int year);
        List<MeterSummary> GetWeeklySummary(string meterId, int year);
        List<MeterSummary> GetMonthlySummary(string meterId, int year);
    }
} 

```mermaid
classDiagram
    class MeterReading {
        +DateTime Timestamp
        +string MeterId
        +double Value
    }

    class MeterSummary {
        +string Period
        +double Sum
        +double PeakValue
        +DateTime PeakTime
    }

class ISummaryStrategy  {
        <<Interface>>
        +string GetPeriodFormat(DateTime date)
        +string GetPeriodLabel(DateTime date)
        +DateTime GetNextPeriod(DateTime date)
        +bool IsInPeriod(DateTime date, DateTime periodStart)
        +DateTime GetPeriodStart(DateTime date)
    }
   

    class MeterDataService {
        -_readings : List~MeterReading~
        -_loaded : bool
        #dataPath : string
        -_dailyStrategy : ISummaryStrategy
        -_weeklyStrategy : ISummaryStrategy
        -_monthlyStrategy : ISummaryStrategy
        +MeterDataService()
        +Task LoadAsync()
        +List~string~ GetMeterIds()
        +List~int~ GetYears(string meterId)
        -List~MeterSummary~ GetSummary(string meterId, int year, ISummaryStrategy strategy)
        +List~MeterSummary~ GetDailySummary(string meterId, int year)
        +List~MeterSummary~ GetWeeklySummary(string meterId, int year)
        +List~MeterSummary~ GetMonthlySummary(string meterId, int year)
    }

     class IMeterDataService  {
        <<Interface>>
        +Task LoadAsync()
        +List~string~ GetMeterIds()
        +List~int~ GetYears(string meterId)
        +List~MeterSummary~ GetDailySummary(string meterId, int year)
        +List~MeterSummary~ GetWeeklySummary(string meterId, int year)
        +List~MeterSummary~ GetMonthlySummary(string meterId, int year)
    }

    class DailySummaryStrategy {
        +string GetPeriodFormat(DateTime date)
        +string GetPeriodLabel(DateTime date)
        +DateTime GetNextPeriod(DateTime date)
        +bool IsInPeriod(DateTime date, DateTime periodStart)
        +DateTime GetPeriodStart(DateTime date)
    }

    class WeeklySummaryStrategy {
        +string GetPeriodFormat(DateTime date)
        +string GetPeriodLabel(DateTime date)
        +DateTime GetNextPeriod(DateTime date)
        +bool IsInPeriod(DateTime date, DateTime periodStart)
        +DateTime GetPeriodStart(DateTime date)
    }

    class MonthlySummaryStrategy {
        +string GetPeriodFormat(DateTime date)
        +string GetPeriodLabel(DateTime date)
        +DateTime GetNextPeriod(DateTime date)
        +bool IsInPeriod(DateTime date, DateTime periodStart)
        +DateTime GetPeriodStart(DateTime date)
    }

    MeterDataService ..|> IMeterDataService : implements
    MeterDataService o-- ISummaryStrategy : uses
    
    
    ISummaryStrategy <|.. DailySummaryStrategy : implements
    ISummaryStrategy <|.. WeeklySummaryStrategy : implements
    ISummaryStrategy <|.. MonthlySummaryStrategy : implements
    MeterDataService ..> MeterReading : aggregates
    MeterDataService ..> MeterSummary : creates 
    ```
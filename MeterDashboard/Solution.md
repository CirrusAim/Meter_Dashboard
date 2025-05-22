# Meter Dashboard Project

## Project Overview
The goal of this project was to develop a web-based dashboard using Blazor to display and summarize meter reading data loaded from a CSV file. The dashboard needed to:
- Load and parse meter data from a CSV file.
- Provide summaries of consumption data aggregated by different time periods (Daily, Weekly, Monthly).
- Opting to present the data in a user-friendly, modern interface using Bootstrap provided by Blazor with minimalistic pagination and filtering.

## 1. Approach and Planning
The initial approach involved breaking down the requirements into logical layers:
- Data Layer: Handling the reading and parsing of the CSV file.
- Service Layer: Implementing the data aggregation and filtering logic.
- UI Components Layer: Building reusable Blazor components for displaying data (Table) and user controls (dropdowns, datepickers, pagination).
- Page Layer: Integrating the UI components and service layer to create the main dashboard view.

### Initial Plan:
- Project Setup: Scaffold a new Blazor project. Given the potential for future real-time data or more complex server-side processing (as hinted in the scenario's scalability mention), I initially opted for Blazor Server with InteractiveServer render mode.
- Data Modeling: Define C# classes (MeterReading, MeterSummary) to represent the structure of the raw and aggregated data.
- Data Service (MeterDataService): Implement a service class responsible for loading the CSV, parsing the data into MeterReading objects, and providing methods for data retrieval and initial aggregation.
- UI Components:
  - A component for the main dashboard page (Home.razor).
  - A component for displaying the summary data in a table (SummaryTable.razor).
- Integration: Use dependency injection to provide the MeterDataService to the Home.razor page and pass data/parameters between the page and UI components.

## 2. Addressing Ambiguities and Assumptions
Throughout the development, several points required clarification or led to specific assumptions:

### CSV Format
I assumed a consistent timestamp;meter_id;value format per line, which was confirmed during parsing implementation.

### Meaning of "Value" and corresponding aggregation
 - This was a critical point. The initial assumption might be a simple reading at a point in time. However, the clarification "*_The timestamp in the file represents the readout time for consumption since the previous readout_*" fundamentally changed the aggregation logic. This meant value was a delta (previous interval consumption), not an instantaneous reading thereby significantly impacting the summing logic.
 - Thereby confirming the requirement to include readings from "1:00:00 AM - 12:00:00 AM of the next day" for a given day.

### "Modern UI"
This is subjective however, I resorted to using standard HTML elements with Bootstrap CSS classes which were already part of the Blazor template for styling, providing a clean, minimalistic and functional interface.

### Time Zones
Assumed the Unix timestamps in the CSV represented UTC, as is common practice. The conversion DateTimeOffset.FromUnixTimeSeconds(unix).DateTime results in UTC DateTime objects, and all subsequent grouping and filtering logic operates on these UTC times. This is a reasonable assumption unless a specific local time zone was specified.

## 3. Implementation Process and Considerations
**CSV Parsing:** 
- Implemented a method in MeterDataService to read the CSV line by line, split values, and parse long (Unix timestamp) and double (value), handling potential parsing errors gracefully (continue if parsing fails).

**Data Storage:** 
- Stored the parsed MeterReading objects in a List<MeterReading> in the service, loaded once (loaded flag) to avoid repeated file access.


**Aggregation Logic:**
- Implemented a Strategy Pattern for period-based aggregation:
  - Created `ISummaryStrategy` interface defining period calculation methods
  - Implemented concrete strategies for Daily, Weekly, and Monthly aggregations
  - Each strategy handles its specific period boundaries and formatting

**Strategy Pattern Implementation:**
- `ISummaryStrategy` interface defines:
  - `GetPeriodFormat`:  How to format the period (e.g., "yyyy-MM-dd" for daily)
  - `GetPeriodLabel`: How to display the period to users
  - `GetNextPeriod`: How to calculate the start of the next period
  - `IsInPeriod`: Determines if a reading belongs to a period based on a given strategy
  - `GetPeriodStart`: Calculates the start of a period reading for a given strategy

**Period Calculation Logic:**
- Daily Strategy:
  - Manages daily boundaries 
  - Period starts at 01:00 AM of the current day
  - Includes readings until 23:59 PM of the current day
  - Plus the 00:00 AM reading of the next day
  - Example: June 1st, includes readings from 01:00 AM June 1st to 23:59 PM June 1st, plus 00:00 AM June 2nd

- Weekly Strategy:
  - Manages weekly boundaries 
  - Period starts at 01:00 AM of Monday
  - Includes readings until 23:59 PM of Sunday
  - Plus the 00:00 AM reading of next Monday
  - Example: Week 22 includes readings from 01:00 AM Monday to 23:59 PM Sunday, plus 00:00 AM next Monday

- Monthly Strategy:
  - Manages monthly boundaries 
  - Period starts at 01:00 AM of the 1st day of the month
  - Includes readings until 23:59 PM of the last day of the month
  - Plus the 00:00 AM reading of the 1st day of next month
  - Example: June includes readings from 01:00 AM June 1st to 23:59 PM June 30th, plus 00:00 AM July 1st

**MeterDataService Implementation:**
- Uses strategy pattern for aggregation:
  - Maintains instances of each strategy type
  - Period calculation logic is encapsulated in strategy classes
  - Shows effective use of dependency injection for strategies
  - Illustrates how the strategy pattern simplifies adding new aggregation types
- `GetSummary` method uses the appropriate strategy to:
  - Get unique period starts depending on strategy
  - For each period, it uses strategy's `IsInPeriod` to filter readings
  - Calculate sum, peak value, and peak time for the period
- Public methods (`GetDailySummary`, `GetWeeklySummary`, `GetMonthlySummary`) delegate to `GetSummary` with appropriate strategy


**Strategy Pattern Benefits:**
- Clean separation of period calculation logic
- Easy to add new aggregation types
- Consistent period boundary handling
- Improved testability of period calculations


## 4. Future Enhancements

1. Strategy Pattern Extensions:
    - Add new aggregation strategies (e.g. average consumption of every meter, Highest/Lowest recording of every meter)

2. Charts
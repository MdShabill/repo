// Path: ConstructionApplication.Core/DataModels/Dashboard/MonthlyActivityStats.cs
namespace ConstructionApplication.Core.DataModels.Dashboard
{
    public class MonthlyActivityStats
    {
        public decimal MonthlySpend { get; set; }
        public int MonthlyPurchases { get; set; }
        public int MonthlyAttendance { get; set; }
    }
}
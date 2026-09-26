// Path: ConstructionApplication.Repository/Interfaces/IDashboardRepository.cs
using ConstructionApplication.Core.DataModels.Dashboard;
using ConstructionApplication.Core.DataModels.SiteScope;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        DashboardStats GetStats(int siteId);
        List<SiteScope> GetScopeItems(int siteId);

        MonthlyActivityStats GetMonthlyActivity(int siteId, int month, int year);
    }
}
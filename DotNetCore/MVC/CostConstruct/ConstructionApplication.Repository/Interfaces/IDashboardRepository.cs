// Path: ConstructionApplication.Repository/Interfaces/IDashboardRepository.cs
using ConstructionApplication.Core.DataModels.Dashboard;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        DashboardStats GetStats(int siteId);
    }
}
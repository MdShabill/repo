using ConstructionApplication.Core.DataModels.SiteScope;
using ConstructionApplication.Core.DataModels.SiteScopeStatus;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteScopeRepository
    {
        List<SiteScope> GetBySiteId(int siteId);
        List<SiteScopeStatus> GetAllStatuses();
        void SaveScopes(int siteId, List<(int scopeMasterId, int statusId, string? remarks)> scopes);
        void UpdateScopeStatus(int id, int scopeStatusId, string? remarks, DateTime? completedDate);
    }
}
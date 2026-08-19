using ConstructionApplication.Core.DataModels.SiteScope;
using ConstructionApplication.Core.DataModels.SiteScopeMaster;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteScopeMasterRepository
    {
        List<SiteScopeMaster> GetAll();
    }
}
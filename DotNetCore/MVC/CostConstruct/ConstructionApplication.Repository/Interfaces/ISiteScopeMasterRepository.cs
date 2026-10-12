using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteScopeMasterRepository
    {
        List<SiteScopeMaster> GetAll();
    }
}
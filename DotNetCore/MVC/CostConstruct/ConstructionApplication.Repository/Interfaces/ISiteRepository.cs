using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Core.Enums;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteRepository
    {
        public List<Site> GetAllSites();
        public Site GetSiteById(int id);
        public int Update(Site site);
        public int Create(Site site);
        public void Delete(int siteId);
        public List<int> GetServiceProviderIdsByTypes(int siteId, List<ServiceTypes> serviceTypes);
        public void AddAndUpdateSiteServiceProviderBridge(int siteId, ServiceTypes serviceType, List<int> serviceProviderIds);
    }
}

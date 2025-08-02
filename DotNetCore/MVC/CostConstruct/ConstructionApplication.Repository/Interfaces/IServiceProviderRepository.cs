using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.Enums;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IServiceProviderRepository
    {
        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id);
        public List<ServiceProviderName> GetAllServiceProviders();
        public int Add(ServiceProvider serviceProvider);
        public int Update(ServiceProvider serviceProvider);
        public void Delete(int serviceProviderId);
    }
}

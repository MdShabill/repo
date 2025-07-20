using ConstructionApplication.Core.DataModels.ServiceProviders;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IServiceProviderRepository
    {
        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id);
        public int Add(ServiceProvider serviceProvider);
        public int Update(ServiceProvider serviceProvider);
        public void Delete(int serviceProviderId);
    }
}

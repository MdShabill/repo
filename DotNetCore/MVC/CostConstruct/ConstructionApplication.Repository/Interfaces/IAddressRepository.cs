using ConstructionApplication.Core.DataModels.Address;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IAddressRepository
    {
        public void InsertOrUpdateAddress(Address address);
        public void Delete(int serviceProviderId, int? siteId);
    }
}

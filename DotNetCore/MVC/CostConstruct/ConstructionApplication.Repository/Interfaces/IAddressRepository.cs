using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IAddressRepository
    {
        public Address GetBySiteId(int siteId);
        public void Upsert(Address address);
        public void Delete(int serviceProviderId, int? siteId);
    }
}

using ConstructionApplication.DataModels.Address;

namespace ConstructionApplication.Repositories
{
    public interface IAddressRepository
    {
        public void Add(Address address);
        public void InsertOrUpdateAddress(Address address);
        public void Delete(int ContractorId);
    }
}

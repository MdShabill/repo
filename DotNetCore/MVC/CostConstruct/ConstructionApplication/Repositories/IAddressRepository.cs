using ConstructionApplication.DataModels.Address;

namespace ConstructionApplication.Repositories
{
    public interface IAddressRepository
    {
        public void Add(Address address);
        public void Update(Address address);
        public void Delete(int ContractorId);
    }
}

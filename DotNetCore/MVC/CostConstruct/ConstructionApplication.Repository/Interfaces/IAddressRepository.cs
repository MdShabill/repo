using ConstructionApplication.Core.DataModels.Address;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IAddressRepository
    {
        public void Add(Address address);
        public void InsertOrUpdateAddress(Address address);
        public void Delete(int ContractorId);
    }
}

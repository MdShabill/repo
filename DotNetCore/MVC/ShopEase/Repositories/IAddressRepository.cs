using ShopEase.DataModels.Address;

namespace ShopEase.Repositories
{
    public interface IAddressRepository
    {
        public List<Address> GetAllAddress(int customerId);
        public void Add(Address address);
        public void Update(Address address);
    }
}

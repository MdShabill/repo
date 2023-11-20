using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IAddressTypeRepository
    {
        public List<AddressType> GetAllAddresses();
    }
}

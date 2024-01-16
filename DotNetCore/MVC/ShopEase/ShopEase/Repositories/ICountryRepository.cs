using ShopEase.DataModels.Address;

namespace ShopEase.Repositories
{
    public interface ICountryRepository
    {
        public List<Country> GetAllCountries();
    }
}

using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface ICountryRepository
    {
        public List<Country> GetAllCountries();
    }
}

using ConstructionApplication.Core.DataModels.Country;

namespace ConstructionApplication.Repositories
{
    public interface ICountryRepository
    {
        public List<Country> GetAllCountries();
    }
}

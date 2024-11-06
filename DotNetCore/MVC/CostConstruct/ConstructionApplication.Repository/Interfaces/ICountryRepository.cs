using ConstructionApplication.Core.DataModels.Country;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ICountryRepository
    {
        public List<Country> GetAllCountries();
    }
}

using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ICountryRepository
    {
        public List<Country> GetAllCountries();
    }
}

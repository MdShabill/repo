using ConstructionApplication.Core.DataModels.Brands;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IBrandRepository
    {
        public List<Brand> GetAll();
    }
}

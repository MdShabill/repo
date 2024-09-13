using ConstructionApplication.DataModels.Brands;

namespace ConstructionApplication.Repositories
{
    public interface IBrandRepository
    {
        public List<Brand> GetAll();
    }
}

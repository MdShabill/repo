using ConstructionApplication.DataModels.Material;

namespace ConstructionApplication.Repositories
{
    public interface IMaterialRepository
    {
        public List<Material> GetAll();
    }
}

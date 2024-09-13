using ConstructionApplication.DataModels.Material;

namespace ConstructionApplication.Repositories
{
    public interface IMaterialRepository
    {
        public Material GetMaterialInfo(int Id);
        public List<Material> GetAll();
    }
}

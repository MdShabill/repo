using ConstructionApplication.Core.DataModels.Material;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IMaterialRepository
    {
        public Material GetMaterialInfo(int Id);
        public List<Material> GetAll();
    }
}

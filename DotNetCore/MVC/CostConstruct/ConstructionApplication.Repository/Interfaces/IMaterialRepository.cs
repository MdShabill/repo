using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IMaterialRepository
    {
        public Material GetMaterialInfo(int Id);
        public List<Material> GetAll();
    }
}

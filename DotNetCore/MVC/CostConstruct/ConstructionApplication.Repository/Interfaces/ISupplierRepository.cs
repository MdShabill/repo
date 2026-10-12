using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISupplierRepository
    {
        public List<Supplier> GetAll();
    }
}

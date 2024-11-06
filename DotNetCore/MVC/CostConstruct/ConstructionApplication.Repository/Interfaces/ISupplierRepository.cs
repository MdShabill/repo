using ConstructionApplication.Core.DataModels.Suppliers;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISupplierRepository
    {
        public List<Supplier> GetAll();
    }
}

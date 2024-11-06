using ConstructionApplication.Core.DataModels.Suppliers;

namespace ConstructionApplication.Repositories
{
    public interface ISupplierRepository
    {
        public List<Supplier> GetAll();
    }
}

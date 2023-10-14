using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IProductSupplierRepository
    {
        public List<ProductSupplier> GetSuppliers();
    }
}

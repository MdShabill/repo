using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductSupplierRepository
    {
        public List<ProductSupplier> GetSuppliers();
    }
}

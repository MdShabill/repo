using ShopEase.DataModels.Product;

namespace ShopEase.Repositories.Product
{
    public interface IProductRepository
    {
        public List<Products> GetAll();
        public int Add(ProductAdd productAdd);
        public List<Brand> GetBrands();
        public List<Category> GetCategories();
        public List<Supplier> GetSuppliers();
    }
}

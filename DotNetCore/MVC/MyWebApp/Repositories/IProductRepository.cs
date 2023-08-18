using MyWebApp.DataModel;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public Product Get(int id);
        public List<ProductResult> GetProducts(ProductFilter ProductsResult);
        public List<ProductResultsOptional> GetProductsResult(ProductFilterOptional productFilterOptional);
        public List<ProductSize> GetSizes();
        public List<ProductColor> GetColors();
        public List<ProductFabric> GetFabric();
        public List<ProductCategory> GetCategory();
        public int Delete(int id);
        public int PlaceOrder(Order order);
        public int Add(Product product);
        public int Update(Product product);
    }
}

using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public Product Get(int id);
        public List<Product> GetProducts(string productName, int colorId, int sizeId, int minPrice, int maxPrice);
        public List<ProductSizes> GetSizes();
        public List<ProductColor> GetColor();
        public List<ProductFabric> GetFabric();
        public List<ProductCategory> GetCategory();
        public void Delete(int id);
        public void Add(Product product);
        public void Update(Product product);
    }
}

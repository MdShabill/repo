using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface IProductRepository
    {
        public List<Product> Index();
        public Product Get(int id);
        public List<ProductSizes> GetSizesDetails();
        public List<ProductColor> GetcolorDetails();
        public void Delete(int id);
        public void Add(Product product);
        public void Update(Product product);
    }
}

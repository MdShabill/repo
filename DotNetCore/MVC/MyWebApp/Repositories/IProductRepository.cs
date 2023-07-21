using MyWebApp.DataModel;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public ProductVm Get(int id);
        public List<ProductVm> GetProducts(string productName, int colorId, int sizeId, int minPrice, int maxPrice);
        public List<ProductSizesVm> GetSizes();
        public List<ProductColorVm> GetColors();
        public List<ProductFabricVm> GetFabric();
        public List<ProductCategoryVm> GetCategory();
        public int Delete(int id);
        public int Add(Product product);
        public int Update(Product product);
    }
}

using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductCategoryReopsitory
    {
        public List<ProductCategory> GetCategories();
    }
}

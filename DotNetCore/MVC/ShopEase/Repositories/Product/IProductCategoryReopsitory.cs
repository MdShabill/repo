using ShopEase.DataModels.Product;

namespace ShopEase.Repositories.Product
{
    public interface IProductCategoryReopsitory
    {
        public List<ProductCategory> GetCategories();
    }
}

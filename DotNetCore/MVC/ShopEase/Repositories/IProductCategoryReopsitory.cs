using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IProductCategoryReopsitory
    {
        public List<ProductCategory> GetCategories();
    }
}

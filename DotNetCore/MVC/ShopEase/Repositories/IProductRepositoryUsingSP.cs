using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductRepositoryUsingSP
    {
        public Product GetProduct(int id);
        public List<ProductSearchResult> GetProductsResult(ProductFilter productsFilter);
    }
}

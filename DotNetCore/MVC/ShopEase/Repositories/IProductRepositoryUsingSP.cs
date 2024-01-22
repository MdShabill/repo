using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductRepositoryUsingSP
    {
        public Product SPGetProduct(int id);
        public List<ProductSearchResult> SPGetProductsResult(ProductFilter productsFilter);
    }
}

using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetSortedProducts(string? sortColumnName, string? sortOrder);
        public Product GetProduct(int? id, string? imageName);
        public int Add(ProductAdd productAdd);
        public List<ProductSearchResult> GetProductsResult(ProductFilter productsFilter);
    }
}

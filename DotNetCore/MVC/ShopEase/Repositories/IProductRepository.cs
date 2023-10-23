using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetSortedProducts(string? sortColumnName, string? sortOrder);
        public int Add(ProductAdd productAdd);
        public List<ProductSearchResult> GetProductsResult(ProductFilterResult productsFilter);
    }
}

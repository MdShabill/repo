using ShopEase.DataModels.Product;

namespace ShopEase.Repositories.Product
{
    public interface IProductRepository
    {
        public List<Products> GetAll();
        List<Products> GetSortedProducts(string? sortColumnName, string? sortOrder, int pageSize);
        public int Add(ProductAdd productAdd);
    }
}

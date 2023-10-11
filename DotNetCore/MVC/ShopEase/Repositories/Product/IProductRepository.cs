using ShopEase.DataModels.Product;

namespace ShopEase.Repositories.Product
{
    public interface IProductRepository
    {
        public List<Products> GetAll();
        public int Add(ProductAdd productAdd);
    }
}

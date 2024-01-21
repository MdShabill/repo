using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface ISPProductRepository
    {
        public Product SP_GetProduct(int id);
    }
}

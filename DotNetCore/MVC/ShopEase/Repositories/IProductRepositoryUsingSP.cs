using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductRepositoryUsingSP
    {
        public Product SPGetProduct(int id);
    }
}

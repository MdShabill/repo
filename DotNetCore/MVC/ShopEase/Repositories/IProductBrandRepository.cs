using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public interface IProductBrandRepository
    {
        public List<ProductBrand> GetBrands();
    }
}

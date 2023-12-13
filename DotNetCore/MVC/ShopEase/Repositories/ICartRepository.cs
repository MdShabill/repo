using ShopEase.DataModels.Cart;

namespace ShopEase.Repositories
{
    public interface ICartRepository
    {
        public List<Cart> GetAllCart();
        public int Add(Cart cart);
    }
}

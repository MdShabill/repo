using ShopEase.DataModels.Cart;

namespace ShopEase.Repositories
{
    public interface ICartRepository
    {
        public List<Cart> GetMyCart(int customerId);
        public int Add(Cart cart);
    }
}

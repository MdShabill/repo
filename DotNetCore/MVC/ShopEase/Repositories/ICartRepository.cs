using ShopEase.DataModels.Cart;
using System.Data;

namespace ShopEase.Repositories
{
    public interface ICartRepository
    {
        public List<Cart> GetAll(int? customerId);
        public int Add(Cart cart);
        public void Delete(int customerId);
    }
}

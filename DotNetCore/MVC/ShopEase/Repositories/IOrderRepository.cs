using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public int PlaceOrder(Order order);
    }
}

using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetOrderByCustomerId(int customerId);
        public OrderDetail GetOrder(int orderNumber);
        public int PlaceOrder(Order order);
    }
}

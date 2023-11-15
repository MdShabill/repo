using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders(int? customerId);
        public OrderDetail GetOrder(int orderNumber);
        public int AddOrder(Order order);
    }
}

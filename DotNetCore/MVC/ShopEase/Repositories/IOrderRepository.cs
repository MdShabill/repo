using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders(int? customerId);
        public Order GetOrder(int orderNumber);
        public int AddOrder(Order order);
    }
}

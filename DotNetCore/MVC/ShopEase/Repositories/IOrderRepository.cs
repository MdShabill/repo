using ShopEase.DataModels.Order;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders(int? customerId, int? orderNumber);
        //public Order GetOrder(int orderNumber);
        public int AddOrder(Order order);
    }
}

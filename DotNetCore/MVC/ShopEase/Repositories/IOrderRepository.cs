using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders();
        public OrderSummaryDetail GetOrders(int orderNumber);
        public int PlaceOrder(Order order);
    }
}

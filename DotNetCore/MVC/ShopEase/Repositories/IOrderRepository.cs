using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders();
        public OrderDetail GetOrderByOrderId(int id);
        public OrderSummary GetLastOrderSummaryDetails(int orderNumber);
        public int PlaceOrder(Order order);
    }
}

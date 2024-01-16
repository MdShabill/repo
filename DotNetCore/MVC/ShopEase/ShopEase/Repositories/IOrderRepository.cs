using ShopEase.DataModels.OderItem;
using ShopEase.DataModels.Order;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders(int? customerId, int? orderNumber);

        public int AddOrder(Order order);

        public void AddOrderItem(OrderItem orderItem);

        public void UpdateProductQuantity(int productId, int orderedQuantity);
    }
}

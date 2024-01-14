using ShopEase.DataModels.Cart;
using ShopEase.DataModels.OderItem;
using ShopEase.DataModels.Order;
using System.Data;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public List<Order> GetAllOrders(int? customerId, int? orderNumber);

        public int AddOrder(Order order);

        public void AddOrderItem(OrderItem orderItem, int orderId, string orderNumber);

        public void UpdateProductQuantity(int productId, int orderQuantity);

        public void AddOrderItem(List<Cart> carts, int orderId, string orderNumber);
    }
}

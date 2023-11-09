using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IOrderRepository 
    {
        public OrderSummary GetLastOrderSummaryDetails();
        public int PlaceOrder(Order order);
    }
}

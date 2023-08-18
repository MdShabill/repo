using MyWebApp.DataModel;

namespace MyWebApp.Repositories
{
    public interface IOrderRepository
    {
        public int PlaceOrder(Order order);
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IOrderRepository
    {
        public DataTable GetAllOrders();
        public int GetOrdersCount();
        public DataTable GetOrderDetailById(int orderId);
        public DataTable GetOrdersDetailByOrderDate(string orderDateTime);
        public DataTable GetOrdersByAmountRange(int minimumAmount, int maximumAmount);
        public int OrderAdd(OrderDto order);
        public void OrderUpdate(OrderDto order);
    }
}

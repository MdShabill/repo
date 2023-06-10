using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IOrderRepository
    {
        public List<OrderDto> GetAllOrdersAsList();
        public int GetOrdersCount();
        public OrderDto GetOrderDetailById(int orderId);
        public List<OrderDto> GetOrdersByAmountRange(int minimumAmount, int maximumAmount);
        public int OrderAdd(OrderDto order);
        public void OrderUpdate(OrderDto order);
    }
}

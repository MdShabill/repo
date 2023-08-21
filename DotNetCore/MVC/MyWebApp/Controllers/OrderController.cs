using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.DataModel;
using MyWebApp.Repositories;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IMapper _imapper;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderVm, Order>();
            });
            _imapper = configuration.CreateMapper();
        }

        [HttpPost]
        public IActionResult OrderPlace(OrderVm orderVm)
        {
            orderVm.CustomerId = 1;
            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            int affectedRowCount = _orderRepository.PlaceOrder(order);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForBuyNow"] = "Buy now Successful";
            }
            return RedirectToAction("Index", "Product");
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopEase.DataModels;
using ShopEase.Repositories;
using ShopEase.ViewModels;

namespace ShopEase.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IAddressRepository _addressRepository;
        IMapper _imapper;

        public OrderController(IOrderRepository orderRepository, IAddressRepository addressRepository)
        {
            _orderRepository = orderRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderVm, Order>();
                cfg.CreateMap<Order, OrderVm>();
                cfg.CreateMap<OrderSummary, OrderSummaryVm>();
            });
            _imapper = configuration.CreateMapper();
            _addressRepository = addressRepository;
        }

        public IActionResult PlaceOrder()
        {
            List<Address> addresses = _addressRepository.GetAllAddress();
            ViewBag.Addresses = new SelectList(addresses, "Id", "AddressDetail");
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(OrderVm orderVm)
        {
            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedInCustomerId"));
            orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedInProductId"));
            orderVm.Price = decimal.Parse(HttpContext.Session.GetString("LoggedInProductPrice"));

            orderVm.OrderNumber = GenerateRandomOrderNumber();

            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            int affectedRowCount = _orderRepository.PlaceOrder(order);
            if(affectedRowCount > 0)
            {
                ViewBag.SuccessMessage = "Your Order Placed Successfully Done... ";
            }
            return RedirectToAction("OrderSummary");
        }

        private int GenerateRandomOrderNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public IActionResult OrderSummary()
        {
            OrderSummary orderSummary = _orderRepository.GetLastOrderSummaryDetails();
            OrderSummaryVm orderSummaryVm = _imapper.Map<OrderSummary, OrderSummaryVm>(orderSummary);
            return View(orderSummaryVm);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
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
                cfg.CreateMap<OrderSummaryDetail, OrderSummaryDetailVm>();
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
            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));
            orderVm.Price = decimal.Parse(HttpContext.Session.GetString("ProductPrice"));

            orderVm.OrderNumber = GenerateRandomOrderNumber();

            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            int affectedRowCount = _orderRepository.PlaceOrder(order);
            if(affectedRowCount > 0)
            {
                ViewBag.SuccessMessage = "Your Order Placed Successfully Done... ";
            }
            HttpContext.Session.SetInt32("OrderNumber", (int)orderVm.OrderNumber);
            return RedirectToAction("OrderSummary");
        }

        private int GenerateRandomOrderNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public IActionResult OrderSummary()
        {
            int orderNumber = Convert.ToInt32(HttpContext.Session.GetInt32("OrderNumber"));
            OrderSummaryDetail ordersummaryDetail = _orderRepository.GetOrders(orderNumber);
            OrderSummaryDetailVm orderSummaryDetailVm = _imapper.Map<OrderSummaryDetail, OrderSummaryDetailVm>(ordersummaryDetail);
            return View(orderSummaryDetailVm);
        }

        public IActionResult MyOrders()
        {
            List<Order> orders = _orderRepository.GetAllOrders();
            List<OrderVm> ordersVm = _imapper.Map<List<Order>, List<OrderVm>>(orders);
            return View(ordersVm);
        }

        public IActionResult OrderDetail(int orderNumber)
        {
            OrderSummaryDetail ordersummaryDetail = _orderRepository.GetOrders(orderNumber);
            OrderSummaryDetailVm orderSummaryDetailVm = _imapper.Map<OrderSummaryDetail, OrderSummaryDetailVm>(ordersummaryDetail);
            return View(orderSummaryDetailVm);
        }   
    }
}

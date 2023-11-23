using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopEase.DataModels;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using static NuGet.Packaging.PackagingConstants;

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
                cfg.CreateMap<Order, OrderSummaryVm>();
                cfg.CreateMap<Order, OrderDetailVm>();
            });
            _imapper = configuration.CreateMapper();
            _addressRepository = addressRepository;
        }

        public IActionResult MyOrder()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            if(customerId > 0)
            {
                List<Order> order = _orderRepository.GetAllOrders(null, customerId);
                List<OrderVm> orderVm = _imapper.Map<List<Order>, List<OrderVm>>(order);
                return View(orderVm);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult OrderSummary()
        {
            int orderNumber = Convert.ToInt32(HttpContext.Session.GetInt32("OrderNumber"));
            List<Order> order = _orderRepository.GetAllOrders(orderNumber, null);
            if (order != null && order.Count > 0)
            {
                Order firstOrder = order[0];
                OrderSummaryVm orderSummaryVm = _imapper.Map<Order, OrderSummaryVm>(firstOrder);

                return View(orderSummaryVm);
            }
            return View("NoOrdersFound");
        }

        public IActionResult OrderDetail(int? orderNumber, int? customerId = null)
        {
            List<Order> order = _orderRepository.GetAllOrders(orderNumber, customerId);
            if (order != null && order.Count > 0)
            {
                Order firstOrder = order[0];
                OrderDetailVm orderDetailVm = _imapper.Map<Order, OrderDetailVm>(firstOrder);

                return View(orderDetailVm);
            }
            return View("NoOrdersFound");
        }

        public IActionResult PlaceOrder()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            if(customerId > 0)
            {
                List<Address> addresses = _addressRepository.GetAllAddress(customerId);
                ViewBag.Address = new SelectList(addresses, "Id", "AddressDetail");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult PlaceOrder(OrderVm orderVm)
        {
            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));
            orderVm.Price = decimal.Parse(HttpContext.Session.GetString("ProductPrice"));

            orderVm.OrderNumber = GenerateRandomOrderNumber();

            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            int affectedRowCount = _orderRepository.AddOrder(order);
            if (affectedRowCount > 0)
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
    }
}

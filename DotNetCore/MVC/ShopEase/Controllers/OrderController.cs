using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopEase.DataModels;
using ShopEase.DataModels.Address;
using ShopEase.DataModels.Cart;
using ShopEase.DataModels.Customer;
using ShopEase.DataModels.OderItem;
using ShopEase.DataModels.Order;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using ShopEase.ViewModels.Cart;
using System.Data;
using System.Text.RegularExpressions;
using System.Transactions;

namespace ShopEase.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IAddressRepository _addressRepository;
        ICardDetailRepository _cardDetailRepository;
        ICartRepository _cartRepository;
        IMapper _imapper;

        public OrderController(IOrderRepository orderRepository, IAddressRepository addressRepository, 
                               ICardDetailRepository cardDetailRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderVm, Order>();
                cfg.CreateMap<Order, OrderVm>();
                cfg.CreateMap<Order, OrderSummaryVm>();
                cfg.CreateMap<Order, OrderDetailVm>();
                cfg.CreateMap<OrderVm, CardDetail>();
            });
            _imapper = configuration.CreateMapper();
            _addressRepository = addressRepository;
            _cardDetailRepository = cardDetailRepository;
            _cartRepository = cartRepository;
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
            return RedirectToAction("Login", "Account"); 
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
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult PlaceOrder()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            if(customerId > 0)
            {
                ViewBag.TotalPrice = HttpContext.Session.GetInt32("TotalPrice");
                
                List<Address> addresses = _addressRepository.GetAllAddress(customerId);
                ViewBag.Address = new SelectList(addresses, "Id", "AddressDetail");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult PlaceOrder(OrderVm orderVm)
        {
            orderVm.CardNumber = orderVm.CardNumber.Replace("-", "").Replace(" ", "");

            orderVm.CardNumber = string.Concat(orderVm.CardNumber.Where(char.IsDigit));

            orderVm.ExpiryDate = new DateTime(orderVm.ExpiryYear, orderVm.ExpiryMonth, 1);

            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));
            orderVm.Price = decimal.Parse(HttpContext.Session.GetString("ProductPrice"));

            orderVm.OrderNumber = GenerateRandomOrderNumber();

            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            order.OrderItem = new OrderItem
            {
                ProductId = orderVm.ProductId,
                Quantity = orderVm.Quantity,
            };
            order.OrderId = _orderRepository.AddOrder(order);
            if (order.OrderId > 0)
            {
                _orderRepository.AddOrderItem(order.OrderItem, order.OrderId, order.OrderNumber);

                _orderRepository.UpdateProductQuantity(order.ProductId, order.Quantity);

                CardDetail cardDetail = _imapper.Map<OrderVm, CardDetail>(orderVm);
                cardDetail.OrderId = order.OrderId;

                _cardDetailRepository.AddCardDetail(cardDetail);

                ViewBag.SuccessMessage = "Your Order Placed Successfully Done... ";
            }
            HttpContext.Session.SetInt32("OrderNumber", orderVm.OrderNumber);
            return RedirectToAction("OrderSummary");
        }

        [HttpPost]
        public IActionResult PlaceOrderFromCart(OrderVm orderVm)
        {
            orderVm.CardNumber = orderVm.CardNumber.Replace("-", "").Replace(" ", "");

            orderVm.CardNumber = string.Concat(orderVm.CardNumber.Where(char.IsDigit));

            if (orderVm.CardNumber.Length != 16)
            {
                ViewBag.ErrorMessage = "Invalid Card Number";
                return View();
            }

            orderVm.ExpiryDate = new DateTime(orderVm.ExpiryYear, orderVm.ExpiryMonth, 1);

            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            orderVm.Price = Convert.ToInt32(HttpContext.Session.GetInt32("TotalPrice"));

            orderVm.OrderNumber = GenerateRandomOrderNumber();

            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            order.OrderItem = new OrderItem
            {
                ProductId = orderVm.ProductId,
                Quantity = orderVm.Quantity,
            };
            order.OrderId = _orderRepository.AddOrder(order);
            if (order.OrderId > 0)
            {
                List<Cart> carts = _cartRepository.GetMyCart(order.CustomerId);

                _orderRepository.AddOrderItem(carts, order.OrderId, order.OrderNumber);

                _cartRepository.Delete(order.CustomerId);

                _orderRepository.UpdateProductQuantity(order.ProductId, order.Quantity);

                CardDetail cardDetail = _imapper.Map<OrderVm, CardDetail>(orderVm);
                cardDetail.OrderId = order.OrderId;

                _cardDetailRepository.AddCardDetail(cardDetail);

                ViewBag.SuccessMessage = "Your Order Placed Successfully Done... ";
            }
            HttpContext.Session.SetInt32("OrderNumber", orderVm.OrderNumber);
            return RedirectToAction("OrderSummary");
        }

        private int GenerateRandomOrderNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}

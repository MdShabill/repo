using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopEase.DataModels;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using System.Transactions;
using static NuGet.Packaging.PackagingConstants;

namespace ShopEase.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IAddressRepository _addressRepository;
        ICardDetailRepository _cardDetailRepository;
        IMapper _imapper;

        public OrderController(IOrderRepository orderRepository, IAddressRepository addressRepository, 
                               ICardDetailRepository cardDetailRepository)
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
            _cardDetailRepository = cardDetailRepository;
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
            return View();
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
            using (TransactionScope transactionScope = new())
            {
                try
                {
                    //int expiryMonth = int.Parse(HttpContext.Request.Form["ExpiryDateMonth"]);
                    //int expiryYear = int.Parse(HttpContext.Request.Form["ExpiryDateYear"]);

                    orderVm.ExpiryDate = new DateTime(orderVm.ExpiryYear, orderVm.ExpiryMonth, 1);
                    
                    orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
                    orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));
                    orderVm.Price = decimal.Parse(HttpContext.Session.GetString("ProductPrice"));

                    orderVm.OrderNumber = GenerateRandomOrderNumber();

                    Order order = _imapper.Map<OrderVm, Order>(orderVm);
                    order.OrderId = _orderRepository.AddOrder(order);
                    if (order.OrderId > 0)
                    {
                        CardDetail cardDetail = new()
                        {
                            OrderId = order.OrderId,
                            CustomerId = orderVm.CustomerId,
                            NickName = orderVm.NickName,
                            CardNumber = orderVm.CardNumber,
                            ExpiryDate = orderVm.ExpiryDate,
                            CVV = orderVm.CVV,
                        };
                        _cardDetailRepository.AddCardDetail(cardDetail);
                        ViewBag.SuccessMessage = "Your Order Placed Successfully Done... ";
                        transactionScope.Complete();
                    }
                    HttpContext.Session.SetInt32("OrderNumber", orderVm.OrderNumber);
                    return RedirectToAction("OrderSummary");
                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();
                }
            }
            return View();
        }

        private int GenerateRandomOrderNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}

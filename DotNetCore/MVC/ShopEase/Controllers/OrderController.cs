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
            });
            _imapper = configuration.CreateMapper();
            _addressRepository = addressRepository;
        }

        public IActionResult PlaceOrder()
        {
            List<Address> addresses = _addressRepository.GetAllAddress();
            ViewBag.Addresses = new SelectList(addresses, "Id", "PinCode");
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(OrderVm orderVm)
        {
            orderVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedInCustomerId"));
            orderVm.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedInProductId"));
            orderVm.Price = decimal.Parse(HttpContext.Session.GetString("LoggedInProductPrice"));
            
            Order order = _imapper.Map<OrderVm, Order>(orderVm);
            int affectedRowCount = _orderRepository.PlaceOrder(order);
            if(affectedRowCount > 0)
            {
                ViewBag.SuccessMessage = "Order Placed SuccessFul ";
            }
            return RedirectToAction("Index", "Product");
        }
    }
}

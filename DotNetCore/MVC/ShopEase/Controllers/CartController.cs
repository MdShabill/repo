using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using ShopEase.DataModels.Address;
using ShopEase.DataModels.Cart;
using ShopEase.DataModels.Order;
using ShopEase.DataModels.Person;
using ShopEase.DataModels.Product;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using ShopEase.ViewModels.Cart;

namespace ShopEase.Controllers
{
    public class CartController : Controller
    {
        ICartRepository _cartRepository;
        IMapper _imapper;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddToCartVm, Cart>();
                cfg.CreateMap<CartVm, Cart>();
                cfg.CreateMap<Cart, CartVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            //List<int> quantities = new List<int> { 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            //ViewBag.Quantities = new SelectList(quantities, "Quantity", "Quantity");

            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            List<Cart> cart = _cartRepository.GetAll(customerId);
            List<CartVm> cartVm = _imapper.Map<List<Cart>, List<CartVm>>(cart);

            int totalItems = cartVm.Count;
            int totalPrice = cartVm.Sum(item => item.Price);
            HttpContext.Session.SetInt32("TotalPrice", totalPrice);

            ViewBag.TotalItems = totalItems;
            TempData["TotalPrice"] = totalPrice;
            return View(cartVm);
        }

        [HttpPost]
        public JsonResult AddToCart(AddToCartVm cartVm)
        {
            Cart cart = _imapper.Map<AddToCartVm, Cart>(cartVm);
            cart.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));

            int affaffectedRowCount = _cartRepository.Add(cart);
            if (affaffectedRowCount > 0)
            {
                TempData["SuccessMessage"] = "The Item Has Been Added To Your Cart";
            }
            return Json(cartVm);
        }
    }
}

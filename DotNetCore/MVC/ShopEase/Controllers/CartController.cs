using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ShopEase.DataModels.Cart;
using ShopEase.DataModels.Order;
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
                cfg.CreateMap<CartVm, Cart>();
                cfg.CreateMap<Cart, CartVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Cart> cart = _cartRepository.GetAllCart();
            List<CartVm> cartVm = _imapper.Map<List<Cart>, List<CartVm>>(cart);

            int totalItems = cartVm.Count;
            int totalPrice = cartVm.Sum(item => item.Price);

            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPrice = totalPrice;
            return View(cartVm);
        } 

        [HttpPost]
        public IActionResult AddToCart(CartVm cartVm)
        {
            cartVm.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            Cart cart = _imapper.Map<CartVm, Cart>(cartVm);
            int affaffectedRowCount = _cartRepository.Add(cart);
            if (affaffectedRowCount > 0)
            {
                TempData["SuccessMessage"] = "The Item Has Been Added To Your Cart";
            }
            return RedirectToAction("View", "Product", new {id = cartVm.ProductId});
        }
    }
}

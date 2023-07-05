using Microsoft.AspNetCore.Mvc;
using MyWebApp.Repositories;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    public class Product1Controller : Controller
    {
        IProduct1Repository _product1Repository;

        public Product1Controller(IProduct1Repository product1Repository)
        {
            _product1Repository = product1Repository;
        }

        public IActionResult ProductSearch()
        {
            return View();
        }
        
        public IActionResult ProductSearchResult(string productName)
        {
            List<Product1> product1 = _product1Repository.GetProduct(productName);
            return View("ProductSearchResult", product1);
        }
    }
}

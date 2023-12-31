using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels.Person;

namespace ShopEase.Controllers
{
    public class DemoJavaScript : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxMethod(string productId, int quantity)
        {
            Product person = new Product
            {
                ProductId = productId,
                Quantity = quantity,
                DateTime = DateTime.Now.ToString()
            };
            return Json(person);
        }
    }
}

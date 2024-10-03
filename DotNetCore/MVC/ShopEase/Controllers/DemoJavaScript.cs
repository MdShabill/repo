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
            Product product = new()
            {
                ProductId = productId,
                Quantity = quantity,
                DateTime = DateTime.Now.ToString()
            };
            return Json(product);
        }

        //public IActionResult JSDivPopupDemo(string fullName, string gender, string email)
        //{
        //    Customer customer = new Customer
        //    {
        //        FullName = fullName,
        //        Gender = gender,
        //        Email = email,
        //    };
        //    return View(customer);
        //}
    }
}

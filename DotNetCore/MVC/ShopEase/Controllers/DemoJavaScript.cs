﻿using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels.Person;
using ShopEase.DataModels.ProductDemo;

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
            Product product = new Product
            {
                ProductId = productId,
                Quantity = quantity,
                DateTime = DateTime.Now.ToString()
            };
            return Json(product);
        }

        public JsonResult JSDivPopupDemo(string fullName, string gender, string email)
        {
            Customer customer = new Customer
            {
                FullName = fullName,
                Gender = gender,
                Email = email,
            };
            return Json(customer);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyWebApp.DataModel;
using MyWebApp.ViewModels;
using MyWebApp.Enums;
using System.Text.RegularExpressions;
using System.Transactions;
using MyWebApp.Helpers;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Http;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Controllers
{
    public class AccountController : Controller
    {
        ICustomerRepository _customerRepository;
        IMapper _imapper;

        public AccountController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerVm>();
            });
            _imapper = configuration.CreateMapper();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password) 
        {
            Customer customer = _customerRepository.GetCustomerDetailsByEmailAndPassword(email, password);
            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);
            if (customer is null)
            {
                _customerRepository.UpdateOnLoginFailed(email);

                ViewBag.ErrorMessage = "Invalid Email and Password ";
                return View(customerVm);
            }
            
            _customerRepository.UpdateOnLoginSuccessful(email);
            
            HttpContext.Session.SetInt32("LoggedInCustomerId", customerVm.Id);
            HttpContext.Session.SetString("LoggedInCustomerEmail", customerVm.Email);
            HttpContext.Session.SetString("LoggedInCustomerFirstName", customerVm.FirstName);
            HttpContext.Session.SetString("LoggedInCustomerLastName", customerVm.LastName);
            HttpContext.Session.SetString("LoggedInCustomerMobile", customerVm.Mobile);

            return RedirectToAction("ProductSearchOptional", "Product");
        }
    }
}

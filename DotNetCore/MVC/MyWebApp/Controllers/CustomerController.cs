using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace MyWebApp.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Customer> customers = _customerRepository.GetAll();

            ViewBag.customerCount = customers.Count;
            return View("Index", customers);
        }

        [HttpGet]
        public IActionResult View(int Id)
        {
            Customer customer = _customerRepository.Get(Id);
            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Customer Customer = _customerRepository.Get(Id);
            return View(Customer);
        }

        public IActionResult CustomerSearch()
        {
            return View();
        }

        public IActionResult CustomerSearchResult(Customer customerFilter)
        {
            List<Customer> customers = _customerRepository.GetCustomers(customerFilter.FirstName, customerFilter.LastName, 
                                                                     (int)customerFilter.Gender);
            return View("CustomerSearchResult", customers);
        }

        public IActionResult CustomerSearchOptional()
        {
            return View();
        }

        public IActionResult CustomerSearchResultOptional(Customer? customerFilter)
        {
            List<Customer> customers = _customerRepository.GetCustomersOptional(customerFilter.FirstName, 
                                        customerFilter.LastName, (int)customerFilter.Gender);
            return View("CustomerSearchResultOptional", customers);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            int deletedRow = _customerRepository.Delete(id);
            if (deletedRow > 0)
            {
                TempData["SuccessMessageForDelete"] = "Customer Record Delete Successful";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            string errorMessage = validateCustomerRegisterOrUpdate(customer, true);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.errorMssage = errorMessage;
            }

            int affectedRowCount = _customerRepository.Register(customer);

            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForRegister"] = "Customer Register Successful";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            string errorMessage = validateCustomerRegisterOrUpdate(customer, true);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.errorMssage = errorMessage;
            }

            int affectedRowCount = _customerRepository.Update(customer);

            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForUpdate"] = "Customer Update Successful";
            }
            return RedirectToAction("Index");
        }

        private string validateCustomerRegisterOrUpdate(Customer customer, bool IsUpdate = false)
        {
            string errorMessage = "";

            if (IsUpdate == true)
            {
                if (customer.Id < 1)
                    errorMessage = "Customer Id Can Not Be Less Then Zero";
            }

            if (string.IsNullOrEmpty(customer.FirstName))
                errorMessage = "Customer First Name Can Not Be Blank";

            else if (customer.FirstName.Length >= 15)
                errorMessage = "Customer First Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(customer.LastName))
                errorMessage = "customer Last Name Can Not Be Blank";

            else if (customer.LastName.Length >= 15)
                errorMessage = "Customer Last Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(customer.Email))
                errorMessage = "Customer Email Can Not Be Blank";

            else if (string.IsNullOrEmpty(customer.Mobile))
                errorMessage = "customer Mobile Can Not Be Blank";

            else if (customer.Mobile.Length != 10)
                errorMessage = "Customer Mobile Number must Be Exactly 10 Digits";

            return errorMessage;
        }
    }    
}

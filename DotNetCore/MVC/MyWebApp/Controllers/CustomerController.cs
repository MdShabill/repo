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

            string successMessageForAdd = ViewBag.SuccessMessageForAdd;
            if (!string.IsNullOrEmpty(successMessageForAdd))
            {
                ViewBag.SuccessMessageForAdd = successMessageForAdd;
            }

            string successMessageForUpdate = ViewBag.SuccessMessageForUpdate;
            if (!string.IsNullOrEmpty(successMessageForAdd))
            {
                ViewBag.SuccessMessageForUpdate = successMessageForUpdate;
            }

            string successMessageForDelete = ViewBag.SuccessMessageForDelete;
            if (!string.IsNullOrEmpty(successMessageForDelete))
            {
                ViewBag.SuccessMessageForDelete = successMessageForDelete;
            }

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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _customerRepository.Delete(id);

            ViewBag.SuccessMessageForDelete = "Customer Delete SuccessFul";
            List<Customer> customers = _customerRepository.GetAll();
            return View("Index", customers);
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

            _customerRepository.Register(customer);

            ViewBag.SuccessMessageForAdd = "Customer Register SuccessFul";
            List<Customer> customers = _customerRepository.GetAll();
            return View("Index", customers);
        }

        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            string errorMessage = validateCustomerRegisterOrUpdate(customer, true);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.errorMssage = errorMessage;
            }

            _customerRepository.Update(customer);

            ViewBag.SuccessMessageForUpdate = "Customer Update SuccessFul";
            List<Customer> customers = _customerRepository.GetAll();
            return View("Index", customers);
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

            else if (customer.Gender == 0)
                errorMessage = "Customer Gender Can Not Be Zero";

            else if (string.IsNullOrEmpty(customer.Email))
                errorMessage = "Customer Email Can Not Be Blank";

            else if (customer.Age > 18)
                errorMessage = "Customer Age Should Be Above 18 years";

            else if (string.IsNullOrEmpty(customer.Mobile))
                errorMessage = "customer Mobile Can Not Be Blank";

            else if (customer.Mobile.Length != 10)
                errorMessage = "Customer Mobile Number must Be Exactly 10 Digits";

            return errorMessage;
        }
    }    
}

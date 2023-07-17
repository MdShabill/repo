using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyWebApp.DataModel;

namespace MyWebApp.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerRepository _customerRepository;
        IMapper _imapper;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerVm>();
                cfg.CreateMap<CustomerVm, Customer>();
                cfg.CreateMap<Customer, CustomerSearchVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Customer> customers = _customerRepository.GetAll();

            List<CustomerVm> customerVm = _imapper.Map<List<Customer>, List<CustomerVm>>(customers);

            ViewBag.customerCount = customers.Count;
            return View("Index", customerVm);
        }

        [HttpGet]
        public IActionResult View(int id)
        {
            Customer customer = _customerRepository.Get(id);

            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);
            return View(customerVm);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Customer Customer = _customerRepository.Get(Id);

            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(Customer);
            return View(customerVm);
        }

        public IActionResult CustomerSearch()
        {
            return View();
        }

        public IActionResult CustomerSearchResult(CustomerSearchVm vmFilter)
        {
            List<Customer> customers = _customerRepository.GetCustomers(vmFilter.FirstName, vmFilter.LastName, 
                                                                     (int)vmFilter.Gender);

            List<CustomerSearchVm> vmfilter = _imapper.Map<List<Customer>, List<CustomerSearchVm>>(customers);
            return View("CustomerSearchResult", vmfilter);
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
        public IActionResult Register([FromBody] CustomerVm customerVm)
        {
            string errorMessage = validateCustomerRegisterOrUpdate(customerVm, true);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.errorMssage = errorMessage;
            }

            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);

            int affectedRowCount = _customerRepository.Register(customer);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForRegister"] = "Customer Register Successful";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(CustomerVm customerVm)
        {
            string errorMessage = validateCustomerRegisterOrUpdate(customerVm, true);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.errorMssage = errorMessage;
            }

            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);

            int affectedRowCount = _customerRepository.Update(customer);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForUpdate"] = "Customer Update Successful";
            }
            return RedirectToAction("Index");
        }

        private string validateCustomerRegisterOrUpdate(CustomerVm customerVm, bool IsUpdate = false)
        {
            string errorMessage = "";

            if (IsUpdate == true)
            {
                if (customerVm.Id < 1)
                    errorMessage = "Customer Id Can Not Be Less Then Zero";
            }

            if (string.IsNullOrEmpty(customerVm.FirstName))
                errorMessage = "Customer First Name Can Not Be Blank";

            else if (customerVm.FirstName.Length >= 15)
                errorMessage = "Customer First Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(customerVm.LastName))
                errorMessage = "customer Last Name Can Not Be Blank";

            else if (customerVm.LastName.Length >= 15)
                errorMessage = "Customer Last Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(customerVm.Email))
                errorMessage = "Customer Email Can Not Be Blank";

            else if (string.IsNullOrEmpty(customerVm.Mobile))
                errorMessage = "customer Mobile Can Not Be Blank";

            else if (customerVm.Mobile.Length != 10)
                errorMessage = "Customer Mobile Number must Be Exactly 10 Digits";

            return errorMessage;
        }
    }    
}

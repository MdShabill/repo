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

                cfg.CreateMap<CustomerSearchVm, CustomerSearch>();
                cfg.CreateMap<Customer, CustomerSearchVm>();

                cfg.CreateMap<CustomerSearchOptionalVm, CustomerSearchOptional>();
                cfg.CreateMap<Customer, CustomerSearchOptionalVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Customer> customers = _customerRepository.GetAll();

            List<CustomerVm> customersVm = _imapper.Map<List<Customer>, List<CustomerVm>>(customers);

            ViewBag.customerCount = customersVm.Count;
            
            return View("Index", customersVm);
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
            //App #1
            //List<Customer> customers = _customerRepository.GetCustomers(vmFilter.FirstName, vmFilter.LastName, 
            //                                                         (int)vmFilter.Gender);

            //App #2
            //List<Customer> customers = _customerRepository.GetCustomers(vmFilter);

            //App #3
            //CustomerSearch customerSearch = new CustomerSearch();
            //customerSearch.FirstName = vmFilter.FirstName;
            //customerSearch.LastName = vmFilter.LastName;
            //customerSearch.Gender= vmFilter.Gender;

            CustomerSearch customerSearch = _imapper.Map <CustomerSearchVm, CustomerSearch>(vmFilter);
            List<Customer> customers = _customerRepository.GetCustomers(customerSearch);

            List<CustomerSearchVm> vmFilterResult = _imapper.Map<List<Customer>, List<CustomerSearchVm>>(customers);

            //List<CustomerSearchVm> vmfilter = _imapper.Map<List<Customer>, List<CustomerSearchVm>>(customers);
            return View(vmFilterResult);
        }

        public IActionResult CustomerSearchOptional()
        {
            return View();
        }

        public IActionResult CustomerSearchResultOptional(CustomerSearchOptionalVm vmOptionalFilter)
        {
            CustomerSearchOptional optionalFilter = _imapper.Map<CustomerSearchOptionalVm, 
                                                    CustomerSearchOptional>(vmOptionalFilter);
            
            List<Customer> customers = _customerRepository.GetCustomersOptional(optionalFilter);

            List<CustomerSearchOptionalVm> vmOptionalFilterResult = _imapper.Map <List<Customer>, 
                                                         List<CustomerSearchOptionalVm>>(customers);

            return View(vmOptionalFilterResult);
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
        public IActionResult Register(CustomerVm customerVm)
        {
            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);

            if (string.IsNullOrWhiteSpace(customerVm.FirstName))
            {
                ViewBag.ErrorMessage = "First Name Can Not Be Balnk";
                return View();
            }

            if (customerVm.FirstName.Length <= 20)
            {
                ViewBag.ErrorMessage = "First Name Should Be Less than 20 Characters";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.LastName))
            {
                ViewBag.ErrorMessage = "Last Name Can Not Be Blank";
                return View();
            }

            if (customerVm.LastName.Length <= 15)
            {
                ViewBag.ErrorMessage = "Last Name Should Be Less Than 15 Characters";
                return View();
            }

            if (!Enum.IsDefined(typeof(GenderType), customerVm.Gender))
            {
                ViewBag.ErrorMessage = "Customer Gender Invalid";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.Email))
            {
                ViewBag.ErrorMessage = "Email Can Not Be Blank";
                return View();
            }

            if (customerVm.DateOfBirth.Year <= 22)
            {
                ViewBag.ErrorMessage = "Date Of Birth Should Be 22 Above";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.Mobile))
            {
                ViewBag.ErrorMessage = "Mobile Number Can Not Be Blank";
                return View();
            }

            if (ModelState.IsValid)
            {
                int affectedRowCount = _customerRepository.Register(customer);
                if (affectedRowCount > 0)
                {
                    TempData["SuccessMessageForRegister"] = "Customer Register Successful";
                }
            }
            return RedirectToAction("Index", customerVm);
        }

        [HttpPost]
        public IActionResult Update(CustomerVm customerVm)
        {
            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);

            if (customerVm.Id < 1)
                return BadRequest("Customer Id Can Not Be Less Than Zero");

            if (!string.IsNullOrWhiteSpace(customerVm.FirstName))
                return BadRequest("Customer First Name Can Not Be Balnk");

            if (customerVm.FirstName.Length <= 20)
                return BadRequest("Customer First Name Should Be 20 Characters");

            if (!string.IsNullOrWhiteSpace(customerVm.LastName))
                return BadRequest("Customer Last Name Can Not Be Blank");

            if (customerVm.LastName.Length <= 15)
                return BadRequest("Customer Last Name Should be 15 Characters");

            if (!Enum.IsDefined(typeof(GenderType), customerVm.Gender))
                return BadRequest("Customer Gender Invalid");

            if (!string.IsNullOrWhiteSpace(customerVm.Email))
                return BadRequest("Customer Email Can Not Be Blank");

            if (customerVm.DateOfBirth.Year <= 22)
                return BadRequest("Customer Date Of Birth Should Be 22 Above");

            if (!string.IsNullOrWhiteSpace(customerVm.Mobile))
                return BadRequest("Customer Mobile Number Can Not Be Blank");

            int affectedRowCount = _customerRepository.Update(customer);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForUpdate"] = "Customer Update Successful";
            }
            return RedirectToAction("Index");
        }
    }    
}

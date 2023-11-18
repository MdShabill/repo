using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using System.Text.RegularExpressions;

namespace ShopEase.Controllers
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
                cfg.CreateMap<CustomerVm, Customer>();
                cfg.CreateMap<Customer, CustomerVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            List<Customer> customers = _customerRepository.GetAll();

            List<CustomerVm> customersVm = _imapper.Map<List<Customer>, List<CustomerVm>>(customers);

            return View(customersVm);
        }

        public IActionResult MyProfile()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            Customer customer = _customerRepository.GetCustomerById(customerId);
            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);
            return View(customerVm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(CustomerVm customerVm)
        {
            if(string.IsNullOrWhiteSpace(customerVm.FullName))
            {
                ViewBag.ErrorMessage = "Customer Full Name Can not be Blank";
            }

            if(customerVm.FullName.Length > 20)
            {
                ViewBag.ErrorMessage = "Customer Name should be 20 Characters or Less";
            }

            if(string.IsNullOrWhiteSpace(customerVm.Mobile))
            {
                ViewBag.ErrorMessage = "Customer Mobile Number Should Not Be Blank";
            }

            if(customerVm.Mobile.Length != 10)
            {
                ViewBag.ErrorMessage = "Mobile Number Should Be exactly 10 Digits";
            }

            if(string.IsNullOrWhiteSpace(customerVm.Email))
            {
                ViewBag.ErrorMessage = "Email Should Not Be Blank";
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(customerVm.Email);
            if (!match.Success)
            {
                ViewBag.ErrorMessage = "Email is Invalid";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.Password))
            {
                ViewBag.ErrorMessage = "Password Should Not Be Blank";
            }

            if (customerVm.Mobile.Length != 10)
            {
                ViewBag.ErrorMessage = "Mobile Number Should Be exactly 10 Digits";
            }

            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);
            int affectedRowCount = _customerRepository.Register(customer);
            if (affectedRowCount > 0)
            {
                ViewBag.SuccessMessage = "Customer Register Successful";
            }
            return View();
        }

        public IActionResult Edit()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            Customer customer = _customerRepository.GetCustomerById(customerId);
            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);
            return View(customerVm);
        }

        [HttpPost]
        public IActionResult Update(CustomerVm customerVm)
        {
            if (customerVm.Id < 1)
            {
                ViewBag.ErrorMessage = "Id Can Not Be Less Than Zero";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.FullName))
            {
                ViewBag.ErrorMessage = "First Name Can Not Be Balnk";
                return View();
            }

            if (customerVm.FullName.Length > 20)
            {
                ViewBag.ErrorMessage = "First Name Should Be 20 Characters or less";
                return View();
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(customerVm.Email);
            if (!match.Success)
            {
                ViewBag.ErrorMessage = "Email is Invalid";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.Password))
            {
                ViewBag.ErrorMessage = "Password Can Not Be Blank ";
            }

            if (string.IsNullOrWhiteSpace(customerVm.Mobile))
            {
                ViewBag.ErrorMessage = "Mobile Number Can Not Be Blank";
                return View();
            }

            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);

            int affectedRowCount = _customerRepository.Update(customer);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessageForUpdate"] = "Customer Update Successful";
            }
            return RedirectToAction("MyProfile");
        }
    }
}

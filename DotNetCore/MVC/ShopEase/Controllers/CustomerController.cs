﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShopEase.DataModels.Address;
using ShopEase.DataModels.Customer;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using ShopEase.ViewModels.Customer;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml.Linq;

namespace ShopEase.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerRepository _customerRepository;
        IAddressRepository _addressRepository;
        ICountryRepository _countryRepository;
        IMapper _imapper;

        public CustomerController(ICustomerRepository customerRepository, IAddressRepository addressRepository,
                                  ICountryRepository countryRepository)
        {
            _customerRepository = customerRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerVm, Customer>();
                cfg.CreateMap<Customer, CustomerVm>();
            });

            _imapper = configuration.CreateMapper();
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
        }

        public IActionResult Index()
        {
            List<Customer> customers = _customerRepository.GetAll();
            List<CustomerVm> customersVm = _imapper.Map<List<Customer>, List<CustomerVm>>(customers);

            return View(customersVm);
        }

        public async Task<IActionResult> ShowMyData()
        {
            //string apiUrl = "https://localhost:7073/api/Doctors/GetAllDoctors";
            string apiUrl = "https://qa-member.astm.org/m1c/api/v1/value";
            List<ShowMyData> showMyDataList = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    showMyDataList = JsonConvert.DeserializeObject<List<ShowMyData>>(jsonResponse);
                }
                else
                {
                    showMyDataList = new List<ShowMyData>();
                }
            }

            ViewBag.ApiResponse = showMyDataList;

            return View();
        }

        //public IActionResult ShowMyDataUSingAJAX()
        //{
        //    return View();
        //}

        //public IActionResult ShowMyApiDataUSingAJAX()
        //{
        //    return View();
        //}

        public IActionResult MyProfile()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            if (customerId > 0)
            {
                Customer customer = _customerRepository.GetCustomerById(customerId);
                CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);
                return View(customerVm);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Add()
        {
            Array enumValues = Enum.GetValues(typeof(Enums.AddressType));
            ViewBag.AddressTypes = new SelectList(enumValues);

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "CountryName");
            return View();
        }

        [HttpPost]
        public IActionResult Add(CustomerVm customerVm)
        {
            Array enumValues = Enum.GetValues(typeof(Enums.AddressType));
            ViewBag.AddressTypes = new SelectList(enumValues);

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "CountryName");


            ////rule 1
            //if (!string.IsNullOrWhiteSpace(customerVm.FullName))
            //{

            //    //rule 2
            //    if (string.IsNullOrWhiteSpace(customerVm.FullName))
            //    {
            //        //rule 3
            //        if (string.IsNullOrWhiteSpace(customerVm.FullName))
            //        {
            //            //ret
            //        }
            //        else
            //        {
            //            ViewBag.ErrorMessage = "Invalid Full Name";
            //            return View();
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessage = "Invalid Full Name";
            //        return View();
            //    }
            //}
            //else
            //{
            //    ViewBag.ErrorMessage = "Invalid Full Name";
            //    return View();
            //}


            //#rule 1
            if (string.IsNullOrWhiteSpace(customerVm.FullName))
            {
                ViewBag.ErrorMessage = "Invalid Full Name";
                return View();
            }

            //#Rule 2
            string[] nameParts = customerVm.FullName.Split(' ');
            //if ((nameParts.Length == 2 || nameParts.Length == 3) != true)
            //if(   (nameParts.Length > 1 || nameParts.Length < 2) != true)
            
            if (!(nameParts.Length > 1 || nameParts.Length < 2))
            {
                ViewBag.ErrorMessage = "Invalid Full Name";
                return View();
            }

            //#Rule 3
            for (int i = 0; i < nameParts.Length; i++)
            {
                if (nameParts[i].Length <= 3) 
                {
                    ViewBag.ErrorMessage = "Invalid Full Name";
                    return View();
                }
            }

            //now here full name is valid and get it into variables
            string fistName = nameParts[0];
            string middleName, lastName ;

            if (nameParts.Length == 3)
            {
                middleName= nameParts[1];
                lastName = nameParts[2];
            }
            else
            {
                lastName = nameParts[1];
            }
          
            if (string.IsNullOrWhiteSpace(customerVm.Mobile))
            {
                ViewBag.ErrorMessage = "Customer Mobile Number Should Not Be Blank";
                return View();
            }

            if (customerVm.Mobile.Length != 10)
            {
                ViewBag.ErrorMessage = "Mobile Number Should Be exactly 10 Digits";
                return View();
            }

            if (string.IsNullOrWhiteSpace(customerVm.Email))
            {
                ViewBag.ErrorMessage = "Email Should Not Be Blank";
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
                ViewBag.ErrorMessage = "Password Should Not Be Blank";
                return View();
            }

            if (customerVm.Mobile.Length != 10)
            {
                ViewBag.ErrorMessage = "Mobile Number Should Be exactly 10 Digits";
                return View();
            }

            using (TransactionScope transactionScope = new())
            {
                try
                {
                    Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);
                    customer.Id = _customerRepository.Register(customer);
                    if (customer.Id > 0)
                    {
                        Address address = new()
                        {
                            CustomerId = customer.Id,
                            AddressLine1 = customerVm.AddressLine1,
                            AddressLine2 = customerVm.AddressLine2,
                            PinCode = customerVm.PinCode,
                            CountryId = customerVm.CountryId,
                            AddressTypeId = (int)customerVm.AddressTypeId,
                        };
                        _addressRepository.Add(address);

                        transactionScope.Complete();
                        return RedirectToAction("Success");
                    }
                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();
                }
            }
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Edit()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));
            Customer customer = _customerRepository.GetCustomerById(customerId);
            CustomerVm customerVm = _imapper.Map<Customer, CustomerVm>(customer);

            var enumValues = Enum.GetValues(typeof(Enums.AddressType));
            ViewBag.AddressTypes = new SelectList(enumValues);

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "CountryName", customerVm.CountryId);

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

            if (string.IsNullOrWhiteSpace(customerVm.Mobile))
            {
                ViewBag.ErrorMessage = "Mobile Number Can Not Be Blank";
                return View();
            }

            using (TransactionScope transactionScope = new())
            {
                try
                {
                    Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);
                    int affectedRowCount = _customerRepository.Update(customer);
                    if (affectedRowCount > 0)
                    {
                        Address address = new()
                        {
                            CustomerId = customer.Id,
                            AddressLine1 = customerVm.AddressLine1,
                            AddressLine2 = customerVm.AddressLine2,
                            PinCode = customerVm.PinCode,
                            CountryId = customerVm.CountryId,
                            AddressTypeId = (int)customerVm.AddressTypeId,
                        };
                        _addressRepository.Update(address);

                        transactionScope.Complete();
                        return RedirectToAction("MyProfile");
                    }
                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();
                }
            }
            return View();
        }

        public ActionResult SecondAction()
        {
            if (TempData.ContainsKey("Message"))
            {
                string message = TempData["Message"] as string;
                // Now do something with the message
                ViewBag.Message = message;
            }
            return View();
        }

    }
}
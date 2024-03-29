﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels.Customer;
using ShopEase.Repositories;
using ShopEase.ViewModels;

namespace ShopEase.Controllers
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
            Customer customer = _customerRepository.GetCustomerDetailByEmail(email);
            _imapper.Map<Customer, CustomerVm>(customer);

            if (customer is null)
            {
                ViewBag.ErrorMessage = "Invalid Email Or Password ";
                return View();
            }

            if (customer != null && customer.IsLocked)
            {
                ViewBag.LockedErrorMessage = "Your Account Has Been Locked Kindly Contact With Administrator ";
                return View();
            }

            if (customer.Password != password)
            {
                _customerRepository.UpdateOnLoginFailed(email);

                if (customer.LoginFailedCount > 1)
                {
                    _customerRepository.UpdateIsLocked(email);
                }

                ViewBag.ErrorMessage = "Invalid Email Or Password ";
                return View();
            }

            _customerRepository.UpdateOnLoginSuccessfull(email);

            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("CustomerFullName", customer.FullName);
            HttpContext.Session.SetString("CustomerEmail", customer.Email);
            
            return RedirectToAction("ProductSearch", "Product", customer);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

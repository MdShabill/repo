using AutoMapper;
using ConstructionApplication.Core.DataModels.Usres;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApplication.Controllers
{
    public class AccountController : Controller
    {
        IUserRepository _userRepository;
        IMapper _imapper;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserVm>();
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
            User user = _userRepository.GetUserDetailByEmail(email);
            UserVm userVm = _imapper.Map<UserVm>(user);

            if (user is null)
            {
                ViewBag.ErrorMessage = "Invalid Email Or Password ";
                return View();
            }

            if (user != null && user.IsLocked)
            {
                ViewBag.LockedErrorMessage = "Your Account Has Been Locked Kindly Contact With Administrator ";
                return View();
            }

            if (user.Password != password)
            {
                _userRepository.UpdateOnLoginFailed(email);

                if (user.LoginFailedCount >= 2)
                {
                    _userRepository.UpdateIsLocked(email);
                }   

                ViewBag.ErrorMessage = "Invalid Email Or Password ";
                return View();
            }
            _userRepository.UpdateOnLoginSuccessful(email);

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("Email", user.Email);

            return RedirectToAction("Index", "Site");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}

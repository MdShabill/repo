using AutoMapper;
using ConstructEase.WebApp.ViewModels;
using ConstructionApplication.Core.DataModels.Usres;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _imapper;

        public AccountAPIController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string password)
        {
            User user = _userRepository.GetUserDetailByEmail(email);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid Email Or Password" });
            }

            UserVm userVm = _imapper.Map<UserVm>(user);

            if (user.IsLocked)
            {
                return BadRequest(new
                {
                    message = "Your Account Has Been Locked Kindly Contact With Administrator"
                });
            }

            if (user.Password != password)
            {
                _userRepository.UpdateOnLoginFailed(email);

                if (user.LoginFailedCount >= 2)
                {
                    _userRepository.UpdateIsLocked(email);
                }

                return BadRequest(new { message = "Invalid Email Or Password" });
            }

            _userRepository.UpdateOnLoginSuccessful(email);

            return Ok(userVm);
        }
    }
}
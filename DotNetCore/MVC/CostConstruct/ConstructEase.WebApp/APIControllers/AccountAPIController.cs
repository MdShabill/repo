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
        
        public IActionResult Login(UserVm userVm)
        
        {
            if (userVm == null || string.IsNullOrEmpty(userVm.Email) || string.IsNullOrEmpty(userVm.Password))
                return BadRequest(new { message = "Email and Password are required." });

            User user = _userRepository.GetUserDetailByEmail(userVm.Email);

            if (user == null)
                return BadRequest(new { message = "Invalid Email Or Password" });

            if (user.IsLocked)
                return BadRequest(new { message = "Your Account Has Been Locked. Contact Administrator." });

            if (user.Password != userVm.Password)
            {
                _userRepository.UpdateOnLoginFailed(userVm.Email);
                if (user.LoginFailedCount >= 2)
                    _userRepository.UpdateIsLocked(userVm.Email);

                return BadRequest(new { message = "Invalid Email Or Password" });
            }

            _userRepository.UpdateOnLoginSuccessful(userVm.Email);

            // Only return Name + Email — never send Password back to client
            return Ok(
            new UserVm
            {
                FullName = user.Name,
                Email = user.Email
            });
        }
    }
}
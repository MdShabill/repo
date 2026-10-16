using AutoMapper;
using ConstructEase.WebApp.ViewModels;
using ConstructionApplication.Core.DataModels;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _imapper;
        private readonly ILogger<AccountAPIController> _logger;

        public AccountAPIController(IUserRepository userRepository, ILogger<AccountAPIController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserVm>();
            });

            _imapper = configuration.CreateMapper();
        }


        [HttpPost("login")]
        public IActionResult Login(UserVm userVm)
        {
            _logger.LogInformation("API Action: Login | Invoked: {Time}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (userVm == null ||string.IsNullOrWhiteSpace(userVm.Email) 
                    || string.IsNullOrWhiteSpace(userVm.Password))
                {
                    return BadRequest("Email and password are required.");
                }
                User user = _userRepository.GetUserDetailByEmail(userVm.Email);

                if (user == null)
                {
                    return Unauthorized("Invalid Email Or Password.");
                }

                if (user.IsLocked)
                {
                    return StatusCode(StatusCodes.Status423Locked, "Your account has been locked. Kindly contact the administrator.");
                }

                if (user.Password != userVm.Password)
                {
                    _userRepository.UpdateOnLoginFailed(userVm.Email);

                    if (user.LoginFailedCount >= 2)
                    {
                        _userRepository.UpdateIsLocked(userVm.Email, true);

                        return StatusCode(StatusCodes.Status423Locked, "Your account has been locked. Kindly contact the administrator.");
                    }
                    return Unauthorized("Invalid Email Or Password.");
                }
                _userRepository.UpdateOnLoginSuccessful(userVm.Email);

                return Ok(new UserVm
                {
                    FullName = user.Name,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex ,"API Action: {Action} | Failed", nameof(Login));

                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while processing login.");
            }
            finally
            {
                _logger.LogInformation("API Action: Login" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }
    }
}
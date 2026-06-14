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
        public IActionResult Login([FromBody] UserVm userVm)
        {
            User user =
                _userRepository
                .GetUserDetailByEmail(
                    userVm.Email
                );

            if (user == null)
            {
                return BadRequest(new
                {
                    message =
                    "Invalid Email Or Password"
                });
            }

            if (user.IsLocked)
            {
                return BadRequest(new
                {
                    message =
                    "Your Account Has Been Locked Kindly Contact With Administrator"
                });
            }

            if (
                user.Password
                !=
                userVm.Password
            )
            {
                _userRepository
                    .UpdateOnLoginFailed(
                        userVm.Email
                    );

                if (
                    user.LoginFailedCount >= 2
                )
                {
                    _userRepository
                        .UpdateIsLocked(
                            userVm.Email
                        );
                }

                return BadRequest(new
                {
                    message =
                    "Invalid Email Or Password"
                });
            }

            _userRepository
                .UpdateOnLoginSuccessful(
                    userVm.Email
                );

            var response =
                _imapper.Map<UserVm>(
                    user
                );

            return Ok(response);
        }
    }
}
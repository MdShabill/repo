using AutoMapper;
using ConstructEase.WebApp.ViewModels;
using ConstructionApplication.Core.DataModels.Usres;
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

        public AccountAPIController(IUserRepository userRepository,
                                    ILogger<AccountAPIController> logger)
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
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation("\nAPI Action: {Action} | Invoked: {Time}", nameof(Login),
                    DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                Stopwatch methodTimer = Stopwatch.StartNew();

                User user = _userRepository.GetUserDetailByEmail(userVm.Email);

                methodTimer.Stop();

                _logger.LogInformation("\nMethod: {Method} | Time: {Time} ms",
                    nameof(IUserRepository.GetUserDetailByEmail), methodTimer.ElapsedMilliseconds);

                Stopwatch updateTimer = Stopwatch.StartNew();

                _userRepository.UpdateOnLoginSuccessful(userVm.Email);

                updateTimer.Stop();

                _logger.LogInformation("\nMethod: {Method} | Time: {Time} ms",
                    nameof(IUserRepository.UpdateOnLoginSuccessful), updateTimer.ElapsedMilliseconds);


                return Ok(new UserVm
                {
                    FullName = user.Name,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "API Action: {Action} | Failed", nameof(Login));

                return StatusCode(500, "An error occurred while processing login.");
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation("\nAPI Action: {Action} | Completed | Total Time: {Time} ms", nameof(Login),
                    actionTimer.ElapsedMilliseconds);
            }
        }
    }
}
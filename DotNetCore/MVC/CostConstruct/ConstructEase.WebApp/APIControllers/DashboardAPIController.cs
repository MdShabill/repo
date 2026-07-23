// Path: ConstructEase.WebApp/APIControllers/DashboardAPIController.cs
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardAPIController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardAPIController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("stats")]
        public IActionResult GetStats([FromQuery] int siteId = 0)
        {
            var stats = _dashboardRepository.GetStats(siteId);

            return Ok(new
            {
                totalMaterialSpend = stats.TotalMaterialSpend,

                totalPurchases = stats.TotalPurchases,

                totalAttendanceWorkers = stats.TotalAttendanceWorkers
            });
        }
    }
}
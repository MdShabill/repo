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

            bool hasSiteInfo = siteId > 0 && stats.SiteName != null;

            // ── Budget Calculation ──
            decimal? budgetRemaining = null;
            double? budgetUtilizationPercent = null;

            if (hasSiteInfo && stats.ExpectedBudget.HasValue && stats.ExpectedBudget > 0)
            {
                budgetRemaining = stats.ExpectedBudget.Value - stats.TotalMaterialSpend;
                budgetUtilizationPercent = Math.Min(100,
                    (double)(stats.TotalMaterialSpend / stats.ExpectedBudget.Value * 100));
            }

            // ── Timeline Calculation ──
            int? daysElapsed = null;
            int? daysRemaining = null;
            double? timelineProgressPercent = null;
            bool isOverdue = false;

            if (hasSiteInfo && stats.SiteStartedDate.HasValue)
            {
                daysElapsed = (int)(DateTime.Today - stats.SiteStartedDate.Value).TotalDays;

                if (stats.SiteExpectedCompletion.HasValue)
                {
                    int remaining = (int)(stats.SiteExpectedCompletion.Value - DateTime.Today).TotalDays;
                    daysRemaining = remaining;
                    isOverdue = remaining < 0;

                    double totalDays = (stats.SiteExpectedCompletion.Value - stats.SiteStartedDate.Value).TotalDays;
                    if (totalDays > 0)
                        timelineProgressPercent = Math.Min(100,
                            Math.Max(0, (double)daysElapsed.Value / totalDays * 100));
                }
            }

            // ── Scope Completion % ──
            double scopeCompletionPercent = stats.TotalScopes > 0
                ? Math.Round((double)stats.CompletedScopes / stats.TotalScopes * 100, 1)
                : 0;

            return Ok(new
            {
                // All-time
                totalMaterialSpend = stats.TotalMaterialSpend,
                totalPurchases = stats.TotalPurchases,
                totalAttendanceWorkers = stats.TotalAttendanceWorkers,

                // Overview (siteId = 0)
                totalSites = stats.TotalSites,
                activeSites = stats.ActiveSites,
                totalPendingScopes = stats.TotalPendingScopes,

                // This month
                thisMonthSpend = stats.ThisMonthSpend,
                thisMonthPurchases = stats.ThisMonthPurchases,
                thisMonthAttendance = stats.ThisMonthAttendance,

                // Site info
                hasSiteInfo,
                siteName = stats.SiteName,
                siteStatus = stats.SiteStatus,
                siteStartedDate = stats.SiteStartedDate?.ToString("dd MMM yyyy"),
                siteExpectedCompletion = stats.SiteExpectedCompletion?.ToString("dd MMM yyyy"),

                // Budget
                expectedBudget = stats.ExpectedBudget,
                budgetRemaining,
                budgetUtilizationPercent,

                // Timeline
                daysElapsed,
                daysRemaining,
                timelineProgressPercent,
                isOverdue,

                // Scopes
                totalScopes = stats.TotalScopes,
                completedScopes = stats.CompletedScopes,
                inProgressScopes = stats.InProgressScopes,
                pendingScopes = stats.PendingScopes,
                onHoldScopes = stats.OnHoldScopes,
                notStartedScopes = stats.NotStartedScopes,
                scopeCompletionPercent,

                // Last activity
                lastMaterialActivity = stats.LastMaterialActivity?.ToString("dd MMM yyyy"),
                lastAttendanceActivity = stats.LastAttendanceActivity?.ToString("dd MMM yyyy"),
            });
        }

        [HttpGet("scope-items")]
        public IActionResult GetScopeItems([FromQuery] int siteId)
        {
            if (siteId <= 0)
                return BadRequest(new { message = "Site ID is required." });

            var items = _dashboardRepository.GetScopeItems(siteId);
            return Ok(items);
        }

        [HttpGet("monthly-activity")]
        public IActionResult GetMonthlyActivity( int siteId, int month, int year)
        {
            if (siteId <= 0)
                return BadRequest(new { message = "Site ID is required." });

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Month must be between 1 and 12." });

            if (year < 2000 || year > DateTime.Now.Year)
                return BadRequest(new { message = "Invalid year." });

            var result = _dashboardRepository.GetMonthlyActivity(siteId, month, year);

            return Ok(new
            {
                month = month,
                year = year,
                monthlySpend = result.MonthlySpend,
                monthlyPurchases = result.MonthlyPurchases,
                monthlyAttendance = result.MonthlyAttendance,
            });
        }
    }
}
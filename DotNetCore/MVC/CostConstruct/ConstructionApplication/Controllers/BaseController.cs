using Microsoft.AspNetCore.Mvc;

namespace ConstructionApplication.Controllers
{
    public class BaseController : Controller
    {
        protected int? ValidateSelectedSiteId()
        {
            int? siteId = HttpContext.Session.GetInt32("SelectedSiteId");
            if (siteId == null || siteId <= 0)
                TempData["ErrorMessage"] = "Please select a site before accessing attendance";

            return siteId;
        }

        protected int? ValidateUserId()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
                TempData["ErrorMessage"] = "Your session has expired. Please login again.";

            return userId;
        }
    }
}
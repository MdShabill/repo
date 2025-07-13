using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConstructionApplication.Controllers
{
    public class BaseController : Controller
    {
        protected int SiteId;

        //public BaseController()
        //{
        //    siteId = (int)HttpContext.Session.GetInt32("SelectedSiteId");
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            int? selectedSiteId = HttpContext.Session.GetInt32("SelectedSiteId");
            if (selectedSiteId.HasValue)
            {
                SiteId = selectedSiteId.Value;
            }
            else
            {
                // Optional: handle missing site ID
                // e.g., redirect or set default value
                SiteId = 0;
            }
        }

    }
}
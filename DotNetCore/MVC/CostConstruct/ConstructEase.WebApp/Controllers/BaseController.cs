using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEase.WebApp.Controllers
{
    public class BaseController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        protected int SiteId;

        public BaseController(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            int? selectedSiteId = HttpContext.Session.GetInt32("SelectedSiteId");
            SiteId = selectedSiteId ?? 0;

            var sites = _siteRepository.GetAllSites();
            ViewBag.Site = new SelectList(sites, "Id", "Name", selectedSiteId);
        }
    }
}
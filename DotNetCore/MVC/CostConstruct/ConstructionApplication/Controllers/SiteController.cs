using AutoMapper;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionApplication.Controllers
{
    public class SiteController : Controller
    {
        ISiteRepository _siteRepository;
        IMapper _imapper;

        public SiteController(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Site, SiteVm>();
                cfg.CreateMap<SiteVm, Site>();
            });

            _imapper = configuration.CreateMapper();
        }


        [SessionCheck]
        public IActionResult Index()
        {
            List<Site> sites = _siteRepository.GetAllSites();
            List<SiteVm> siteVm = _imapper.Map<List<Site>,List<SiteVm>>(sites);

            int? selectedSiteId = HttpContext.Session.GetInt32("SelectedSiteId");

            ViewBag.Site = new SelectList(sites, "Id", "Name", selectedSiteId);

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            return View(siteVm);
        }

        public IActionResult NoSiteSelcted()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int Id)
        {
            var selectedSite = _siteRepository.GetSiteById(Id);

            if (selectedSite != null)
            {
                HttpContext.Session.SetInt32("SelectedSiteId", selectedSite.Id);
                HttpContext.Session.SetString("SelectedSiteName", selectedSite.Name);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SiteVm siteVm)
        {
            Site site = _imapper.Map<SiteVm, Site>(siteVm);
            int affectedRowCount = _siteRepository.Create(site);
            if (affectedRowCount > 0)
            {
                TempData["AddSuccessMessage"] = "Add New Site Successful";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);
            if (selectedSite == null)
            {
                return NotFound();
            }

            SiteVm siteVm = _imapper.Map<Site, SiteVm>(selectedSite);
            return View(siteVm);
        }

        [HttpPost]
        public IActionResult Update(SiteVm siteVm)
        {
            Site site = _imapper.Map<SiteVm, Site>(siteVm);
            int affectedRowCount = _siteRepository.Update(site);
            if (affectedRowCount > 0)
            {
                TempData["UpdateSuccessMessage"] = "Site Update Successful";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _siteRepository.Delete(id);
            TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}

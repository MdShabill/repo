using AutoMapper;
using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Core.DataModels.SiteStatus;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace ConstructionApplication.Controllers
{
    public class SiteController : BaseController
    {
        ISiteStatusRepository _siteStatusRepository;
        IAddressRepository _addressRepository;
        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IServiceProviderRepository _serviceProviderRepository;
        ISiteRepository _siteRepository;
        IMapper _imapper;

        public SiteController(ISiteStatusRepository siteStatusRepository,
                              IAddressRepository addressRepository,
                              IAddressTypeRepository addressTypeRepository,
                              ICountryRepository countryRepository,
                              IServiceProviderRepository serviceProviderRepository,
                              ISiteRepository siteRepository) : base(siteRepository)
        {
            _siteRepository = siteRepository;
            _siteStatusRepository = siteStatusRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _serviceProviderRepository = serviceProviderRepository;
            _addressTypeRepository = addressTypeRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.DataModels.Site.Site, SiteVm>();
                cfg.CreateMap<SiteVm, Core.DataModels.Site.Site>();
            });

            _imapper = configuration.CreateMapper();
        }


        [SessionCheck]
        public IActionResult Index()
        {
            List<Core.DataModels.Site.Site> sites = _siteRepository.GetAllSites();
            List<SiteVm> siteVm = _imapper.Map<List<Core.DataModels.Site.Site>,List<SiteVm>>(sites);

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
            DropDownSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(SiteVm siteVm)
        {
            Core.DataModels.Site.Site site = _imapper.Map<SiteVm, Core.DataModels.Site.Site>(siteVm);
            site.Id = _siteRepository.Create(site);

            if (site.Id > 0)
            {
                AddAddressIfPresent(site.Id, siteVm);

                if (siteVm.SelectedMasterMasonIds.Count > 0)
                {
                    _siteRepository.AddSiteServiceProviderBridge(site.Id, ServiceTypes.MasterMasion, siteVm.SelectedMasterMasonIds);
                }
                

                TempData["AddSuccessMessage"] = "Add New Site Successful";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View(siteVm);
        }

        public IActionResult Edit(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);
            if (selectedSite == null)
            {
                return NotFound();
            }

            SiteVm siteVm = _imapper.Map<Core.DataModels.Site.Site, SiteVm>(selectedSite);
            DropDownSelectList();
            return View(siteVm);
        }

        [HttpPost]
        public IActionResult Update(SiteVm siteVm)
        {
            DropDownSelectList();

            Core.DataModels.Site.Site site = _imapper.Map<SiteVm, Core.DataModels.Site.Site>(siteVm);
            int affectedRowCount = _siteRepository.Update(site);
            if (affectedRowCount > 0)
            {
                AddAddressIfPresent(site.Id, siteVm);
                TempData["UpdateSuccessMessage"] = "Your Data updated successfully.";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View("Edit", siteVm);
        }

        [HttpPost]
        public IActionResult Delete(int siteId)
        {
            _addressRepository.Delete(0, siteId);

            _siteRepository.Delete(siteId);
            TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";
            return RedirectToAction("Index");
        }

        private void AddAddressIfPresent(int siteId, SiteVm siteVm)
        {
            if (!string.IsNullOrEmpty(siteVm.AddressLine1) ||
               (siteVm.AddressTypeId.HasValue && siteVm.AddressTypeId > 0) ||
               (siteVm.CountryId.HasValue && siteVm.CountryId > 0) ||
               (siteVm.PinCode.HasValue && siteVm.PinCode > 0))
            {
                Address address = new Address(
                    siteVm.ServiceProviderId,
                    siteVm.AddressLine1,
                    siteVm.AddressTypeId ?? 0,
                    siteVm.CountryId ?? 0,
                    siteVm.PinCode ?? 0,
                    siteId
                );
                _addressRepository.InsertOrUpdateAddress(address);
            }
            else
            {
                _addressRepository.Delete(0, siteId);
            }
        }

        private void DropDownSelectList()
        {
            List<SiteStatus> siteStatuses = _siteStatusRepository.GetAll();
            ViewBag.SiteStatus = new SelectList(siteStatuses, "Id", "Status");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            List<ServiceProviderName> masterMasons = _serviceProviderRepository.GetServiceProviders(ServiceTypes.MasterMasion);
            var masterMasonsServiceProviders = masterMasons
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.Id.ToString(),
                        Text = sp.Name
                    }).ToList();

            ViewBag.Name = new MultiSelectList(masterMasonsServiceProviders, "Value", "Text");

        }
    }

}

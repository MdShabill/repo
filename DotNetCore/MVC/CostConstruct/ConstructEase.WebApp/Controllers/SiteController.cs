using AutoMapper;
using ConstructEase.WebApp.ViewModels;
using ConstructionApplication.Core;
using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Core.DataModels.SiteStatus;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Policy;

namespace ConstructEase.WebApp.Controllers
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
        IMemoryCache _cache;


        public SiteController(ISiteStatusRepository siteStatusRepository,

                              IAddressRepository addressRepository,
                              IAddressTypeRepository addressTypeRepository,
                              ICountryRepository countryRepository,
                              IServiceProviderRepository serviceProviderRepository,
                              ISiteRepository siteRepository,
                              IMemoryCache cache) : base(siteRepository)
        {
            _siteRepository = siteRepository;
            _siteStatusRepository = siteStatusRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _serviceProviderRepository = serviceProviderRepository;
            _addressTypeRepository = addressTypeRepository;
            _cache = cache;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ConstructionApplication.Core.DataModels.Site.Site, SiteVm>();
                cfg.CreateMap<SiteVm, ConstructionApplication.Core.DataModels.Site.Site>();
            });

            _imapper = configuration.CreateMapper();
        }


        [SessionCheck]
        public IActionResult Index()
        {
            const string cacheKey = "AllSites";
            List<ConstructionApplication.Core.DataModels.Site.Site> sites;

            if (!_cache.TryGetValue(cacheKey, out sites))
            {
                sites = _siteRepository.GetAllSites();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _cache.Set(cacheKey, sites, cacheEntryOptions);
            }

            List<SiteVm> siteVm = _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>,List<SiteVm>>(sites);

            int? selectedSiteId = HttpContext.Session.GetInt32("SelectedSiteId");

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            ViewBag.Site = new SelectList(sites, "Id", "Name", selectedSiteId);
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

            var model = new SiteVm
            {
                SelectedMasterMasonIds = new List<int>(),
                SelectedElectricianIds = new List<int>(),
                SelectedLabourIds = new List<int>(),
                SelectedPlumberIds = new List<int>(),
                SelectedCarpenterIds = new List<int>(),
                SelectedTilerIds = new List<int>(),
                StartedDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(SiteVm siteVm)
        {
            if (!ModelState.IsValid)
            {
                DropDownSelectList();
                return View(siteVm);
            }

            ConstructionApplication.Core.DataModels.Site.Site site = _imapper.Map<SiteVm, ConstructionApplication.Core.DataModels.Site.Site>(siteVm);
            site.Id = _siteRepository.Create(site);

            if (site.Id > 0)
            {
                AddAddressIfPresent(site.Id, siteVm);

                if (siteVm.SelectedMasterMasonIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.MasterMasion, siteVm.SelectedMasterMasonIds);
                }

                if (siteVm.SelectedElectricianIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Electrician, siteVm.SelectedElectricianIds);
                }

                if (siteVm.SelectedLabourIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Labour, siteVm.SelectedLabourIds);
                }

                if (siteVm.SelectedPlumberIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Plumber, siteVm.SelectedPlumberIds);
                }

                if (siteVm.SelectedPainterIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Painter, siteVm.SelectedPainterIds);
                }

                if (siteVm.SelectedCarpenterIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Carpenter, siteVm.SelectedCarpenterIds);
                }

                if (siteVm.SelectedTilerIds.Count > 0)
                {
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Tiler, siteVm.SelectedTilerIds);
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

            SiteVm siteVm = _imapper.Map<ConstructionApplication.Core.DataModels.Site.Site, SiteVm>(selectedSite);

            var address = _addressRepository.GetBySiteId(id);
            if (address != null)
            {
                siteVm.AddressLine1 = address.AddressLine1;
                siteVm.AddressTypeId = address.AddressTypeId;
                siteVm.CountryId = address.CountryId;
                siteVm.PinCode = address.PinCode;
            }

            siteVm.SelectedMasterMasonIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.MasterMasion });
            siteVm.SelectedElectricianIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Electrician });
            siteVm.SelectedLabourIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Labour });
            siteVm.SelectedPlumberIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Plumber });
            siteVm.SelectedPainterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Painter });
            siteVm.SelectedCarpenterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Carpenter });
            siteVm.SelectedTilerIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Tiler });

            DropDownSelectList(siteVm.SiteStatusId, siteVm.AddressTypeId, siteVm.CountryId);

            return View(siteVm);
        }

        [HttpPost]
        public IActionResult Update(SiteVm siteVm)
        {
            DropDownSelectList(siteVm.SiteStatusId, siteVm.AddressTypeId, siteVm.CountryId);

            var site = _imapper.Map<SiteVm, ConstructionApplication.Core.DataModels.Site.Site>(siteVm);
            int affectedRowCount = _siteRepository.Update(site);

            if (affectedRowCount > 0)
            {
                AddAddressIfPresent(site.Id, siteVm);

                if (siteVm.SelectedMasterMasonIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.MasterMasion, siteVm.SelectedMasterMasonIds);

                if (siteVm.SelectedElectricianIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Electrician, siteVm.SelectedElectricianIds);

                if (siteVm.SelectedLabourIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Labour, siteVm.SelectedLabourIds);

                if (siteVm.SelectedPlumberIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Plumber, siteVm.SelectedPlumberIds);

                if (siteVm.SelectedPainterIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Painter, siteVm.SelectedPainterIds);

                if (siteVm.SelectedCarpenterIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Carpenter, siteVm.SelectedCarpenterIds);

                if (siteVm.SelectedTilerIds.Count > 0)
                    _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Tiler, siteVm.SelectedTilerIds);

                TempData["UpdateSuccessMessage"] = "Site updated successfully.";
                return RedirectToAction("Index");
            }

            return View("Edit", siteVm);
        }

        [HttpPost]
        public IActionResult Delete(int siteId)
        {
            _siteRepository.Delete(siteId);
            TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";
            return RedirectToAction("Index");
        }

        private void AddAddressIfPresent(int siteId, SiteVm siteVm)
        {
            bool hasValidAddress =
                !string.IsNullOrEmpty(siteVm.AddressLine1) ||
                (siteVm.AddressTypeId.HasValue && siteVm.AddressTypeId > 0) ||
                (siteVm.CountryId.HasValue && siteVm.CountryId > 0) ||
                (siteVm.PinCode.HasValue && siteVm.PinCode > 0);

            if (hasValidAddress)
            {
                Address address = new Address(
                    siteVm.ServiceProviderId,
                    siteVm.AddressLine1,
                    siteVm.AddressTypeId > 0 ? siteVm.AddressTypeId : null,
                    siteVm.CountryId > 0 ? siteVm.CountryId.Value : null,
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

        private void DropDownSelectList(int? SiteStatusId = null, int? addressTypeId = null, int? countryId = null)
        {
            List<SiteStatus> siteStatuses = _siteStatusRepository.GetAll();
            ViewBag.SiteStatus = new SelectList(siteStatuses, "Id", "Status", SiteStatusId);

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name", addressTypeId);

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name", countryId);

            List<ServiceProviderName> allServiceProviders = _serviceProviderRepository.GetAllServiceProviders();

            ViewBag.Name = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.MasterMasion)
                    .Select(sp => new SelectListItem 
                    { 
                        Value = sp.Id.ToString(), 
                        Text = sp.Name 
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Electricians = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Electrician)
                    .Select(sp => new SelectListItem 
                    { 
                        Value = sp.Id.ToString(), 
                        Text = sp.Name 
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Labours = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Labour)
                    .Select(sp => new SelectListItem 
                    { 
                        Value = sp.Id.ToString(), 
                        Text = sp.Name 
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Plumbers = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Plumber)
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.Id.ToString(),
                        Text = sp.Name
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Painters = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Painter)
                    .Select(sp => new SelectListItem
                    {
                        Value = sp.Id.ToString(),
                        Text = sp.Name
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Carpenters = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Carpenter)
                    .Select(sp => new SelectListItem 
                    { 
                        Value = sp.Id.ToString(), 
                        Text = sp.Name 
                    })
                    .ToList(), "Value", "Text");

            ViewBag.Tilers = new MultiSelectList(
                allServiceProviders.Where(sp => sp.ServiceTypeId == (int)ServiceTypes.Tiler)
                    .Select(sp => new SelectListItem 
                    { 
                        Value = sp.Id.ToString(), 
                        Text = sp.Name 
                    })
                    .ToList(), "Value", "Text");
        }
    }
}

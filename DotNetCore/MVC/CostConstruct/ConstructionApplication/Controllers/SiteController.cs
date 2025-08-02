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
            List<ConstructionApplication.Core.DataModels.Site.Site> sites = _siteRepository.GetAllSites();
            List<SiteVm> siteVm = _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>, List<SiteVm>>(sites);

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

            // Get already selected service provider IDs per service type
            siteVm.SelectedMasterMasonIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.MasterMasion });
            siteVm.SelectedElectricianIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Electrician });
            siteVm.SelectedLabourIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Labour });
            siteVm.SelectedPlumberIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Plumber });
            siteVm.SelectedPainterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Painter });
            siteVm.SelectedCarpenterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Carpenter });
            siteVm.SelectedTilerIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes> { ServiceTypes.Tiler });

            DropDownSelectList();
            return View(siteVm);
        }

        [HttpPost]
        public IActionResult Update(SiteVm siteVm)
        {
            DropDownSelectList();

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

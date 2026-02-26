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

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteAPIController : ControllerBase
    {
        ISiteStatusRepository _siteStatusRepository;
        IAddressRepository _addressRepository;
        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IServiceProviderRepository _serviceProviderRepository;
        private readonly ISiteRepository _siteRepository;
        IMapper _imapper;
        IMemoryCache _cache;


        public SiteAPIController(ISiteStatusRepository siteStatusRepository,

                              IAddressRepository addressRepository,
                              IAddressTypeRepository addressTypeRepository,
                              ICountryRepository countryRepository,
                              IServiceProviderRepository serviceProviderRepository,
                              ISiteRepository siteRepository,
                              IMemoryCache cache)
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


        [HttpGet("GetAllSites")]
        public IActionResult GetAllSites()
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

            var siteVm = _imapper.Map<
                List<ConstructionApplication.Core.DataModels.Site.Site>,
                List<SiteVm>>(sites);

            return Ok(siteVm);
        }

        [HttpPost("select-site")]
        public IActionResult SelectSite(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);

            if (selectedSite == null)
                return NotFound(new { message = "Site not found" });

            var response = new
            {
                selectedSiteId = selectedSite.Id,
                selectedSiteName = selectedSite.Name
            };

            return Ok(response);
        }


        [HttpPost("add")]
        public IActionResult Add(SiteVm siteVm)
        {
            if (siteVm == null)
                return BadRequest("Invalid data");

            var site = _imapper.Map<
                SiteVm,
                ConstructionApplication.Core.DataModels.Site.Site>(siteVm);

            site.Id = _siteRepository.Create(site);

            if (site.Id <= 0)
                return StatusCode(500, "Failed to create site");

            // Address
            AddAddressIfPresent(site.Id, siteVm);

            // Service Providers
            if (siteVm.SelectedMasterMasonIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.MasterMasion, siteVm.SelectedMasterMasonIds);

            if (siteVm.SelectedElectricianIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Electrician, siteVm.SelectedElectricianIds);

            if (siteVm.SelectedLabourIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Labour, siteVm.SelectedLabourIds);

            if (siteVm.SelectedPlumberIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Plumber, siteVm.SelectedPlumberIds);

            if (siteVm.SelectedPainterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Painter, siteVm.SelectedPainterIds);

            if (siteVm.SelectedCarpenterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Carpenter, siteVm.SelectedCarpenterIds);

            if (siteVm.SelectedTilerIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Tiler, siteVm.SelectedTilerIds);

            return Ok(new
            {
                message = "Add New Site Successful",
                siteId = site.Id
            });
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);

            if (selectedSite == null)
                return NotFound(new { message = "Site not found" });

            var siteVm = _imapper.Map<
                ConstructionApplication.Core.DataModels.Site.Site,
                SiteVm>(selectedSite);

            // Already selected service provider IDs
            siteVm.SelectedMasterMasonIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.MasterMasion });

            siteVm.SelectedElectricianIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Electrician });

            siteVm.SelectedLabourIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Labour });

            siteVm.SelectedPlumberIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Plumber });

            siteVm.SelectedPainterIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Painter });

            siteVm.SelectedCarpenterIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Carpenter });

            siteVm.SelectedTilerIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Tiler });

            return Ok(siteVm);
        }

        [HttpPost("update")]
        public IActionResult Update(SiteVm siteVm)
        {
            if (siteVm == null)
                return BadRequest("Invalid data");

            var site = _imapper.Map<
                SiteVm,
                ConstructionApplication.Core.DataModels.Site.Site>(siteVm);

            int affectedRowCount = _siteRepository.Update(site);

            if (affectedRowCount <= 0)
                return NotFound(new { message = "Site not found or update failed" });

            // Address update
            AddAddressIfPresent(site.Id, siteVm);

            // Service Providers Update
            if (siteVm.SelectedMasterMasonIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.MasterMasion, siteVm.SelectedMasterMasonIds);

            if (siteVm.SelectedElectricianIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Electrician, siteVm.SelectedElectricianIds);

            if (siteVm.SelectedLabourIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Labour, siteVm.SelectedLabourIds);

            if (siteVm.SelectedPlumberIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Plumber, siteVm.SelectedPlumberIds);

            if (siteVm.SelectedPainterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Painter, siteVm.SelectedPainterIds);

            if (siteVm.SelectedCarpenterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Carpenter, siteVm.SelectedCarpenterIds);

            if (siteVm.SelectedTilerIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Tiler, siteVm.SelectedTilerIds);

            return Ok(new
            {
                message = "Site updated successfully",
                siteId = site.Id
            });
        }

        [HttpDelete("{siteId}")]
        public IActionResult Delete(int siteId)
        {
            if (siteId <= 0)
                return BadRequest();

            _addressRepository.Delete(0, siteId);
            _siteRepository.Delete(siteId);

            return NoContent();
        }

        [HttpPost("AddAddressIfPresent/{siteId}")]
        public IActionResult AddAddressIfPresent(int siteId, SiteVm siteVm)
        {
            if (siteId <= 0)
                return BadRequest("Invalid site id");

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

                return Ok("Address inserted or updated");
            }
            else
            {
                _addressRepository.Delete(0, siteId);
                return Ok("Address deleted");
            }
        }
    }
}

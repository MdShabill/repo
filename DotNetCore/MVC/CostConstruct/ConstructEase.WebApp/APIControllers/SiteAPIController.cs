using AutoMapper;
using ConstructEase.WebApp.APIControllers.APIViewModels;
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
                cfg.CreateMap<ConstructionApplication.Core.DataModels.Site.Site, SiteAPIDTO>();
                cfg.CreateMap<SiteAPIDTO, ConstructionApplication.Core.DataModels.Site.Site>();
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

            var siteApiVm = 
                _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>,List<SiteAPIDTO>>(sites);

            return Ok(siteApiVm);
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
        public IActionResult Add(SiteAPIDTO siteApiVm)
        {
            if (siteApiVm == null)
                return BadRequest("Invalid data");

            var site = 
                _imapper.Map<SiteAPIDTO, ConstructionApplication.Core.DataModels.Site.Site>(siteApiVm);

            site.Id = _siteRepository.Create(site);

            if (site.Id <= 0)
                return StatusCode(500, "Failed to create site");

            // Address
            AddAddressIfPresent(site.Id, siteApiVm);

            // Service Providers
            if (siteApiVm.MasterMasonIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.MasterMasion, siteApiVm.MasterMasonIds);

            if (siteApiVm.ElectricianIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Electrician, siteApiVm.ElectricianIds);

            if (siteApiVm.LabourIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Labour, siteApiVm.LabourIds);

            if (siteApiVm.PlumberIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Plumber, siteApiVm.PlumberIds);

            if (siteApiVm.PainterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Painter, siteApiVm.PainterIds);

            if (siteApiVm.CarpenterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Carpenter, siteApiVm.CarpenterIds);

            if (siteApiVm.TilerIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Tiler, siteApiVm.TilerIds);

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

            var siteApiVm = 
                _imapper.Map<ConstructionApplication.Core.DataModels.Site.Site,SiteAPIDTO>(selectedSite);

            // Already selected service provider IDs
            siteApiVm.MasterMasonIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.MasterMasion });

            siteApiVm.ElectricianIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Electrician });

            siteApiVm.LabourIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Labour });

            siteApiVm.PlumberIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Plumber });

            siteApiVm.PainterIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Painter });

            siteApiVm.CarpenterIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Carpenter });

            siteApiVm.TilerIds =
                _siteRepository.GetServiceProviderIdsByTypes(
                    id, new List<ServiceTypes> { ServiceTypes.Tiler });

            return Ok(siteApiVm);
        }

        [HttpPost("update")]
        public IActionResult Update(SiteAPIDTO siteApiVm)
        {
            if (siteApiVm == null)
                return BadRequest("Invalid data");

            var site = 
                _imapper.Map<SiteAPIDTO,ConstructionApplication.Core.DataModels.Site.Site>(siteApiVm);

            int affectedRowCount = _siteRepository.Update(site);

            if (affectedRowCount <= 0)
                return NotFound(new { message = "Site not found or update failed" });

            // Address update
            AddAddressIfPresent(site.Id, siteApiVm);

            // Service Providers Update
            if (siteApiVm.MasterMasonIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.MasterMasion, siteApiVm.MasterMasonIds);

            if (siteApiVm.ElectricianIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Electrician, siteApiVm.ElectricianIds);

            if (siteApiVm.LabourIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Labour, siteApiVm.LabourIds);

            if (siteApiVm.PlumberIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Plumber, siteApiVm.PlumberIds);

            if (siteApiVm.PainterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Painter, siteApiVm.PainterIds);

            if (siteApiVm.CarpenterIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Carpenter, siteApiVm.CarpenterIds);

            if (siteApiVm.TilerIds?.Count > 0)
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id, ServiceTypes.Tiler, siteApiVm.TilerIds);

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

        private void AddAddressIfPresent(int siteId, SiteAPIDTO siteApiVm)
        {
            if (siteId <= 0)
                return;

            if (!string.IsNullOrEmpty(siteApiVm.AddressLine1) ||
                (siteApiVm.AddressTypeId.HasValue && siteApiVm.AddressTypeId > 0) ||
                (siteApiVm.CountryId.HasValue && siteApiVm.CountryId > 0) ||
                (siteApiVm.PinCode.HasValue && siteApiVm.PinCode > 0))
            {
                Address address = new Address(
                    siteApiVm.Id,
                    siteApiVm.AddressLine1,
                    siteApiVm.AddressTypeId ?? 0,
                    siteApiVm.CountryId ?? 0,
                    siteApiVm.PinCode ?? 0,
                    siteId
                );

                _addressRepository.InsertOrUpdateAddress(address);
            }
            else
            {
                _addressRepository.Delete(0, siteId);
            }
        }
    }
}

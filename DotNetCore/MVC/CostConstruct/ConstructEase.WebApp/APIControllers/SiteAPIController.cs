using AutoMapper;
using ConstructionApplication.Core.DataModels.Address;
using ConstructEase.WebApp.APIControllers.APIViewModels;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteAPIController : ControllerBase
    {
        private readonly ISiteStatusRepository _siteStatusRepository;
        IAddressRepository _addressRepository;
        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IServiceProviderRepository _serviceProviderRepository;
        private readonly ISiteRepository _siteRepository;
        IMapper _imapper;
        IMemoryCache _cache;
        //private object _statusRepository;

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
                cfg.CreateMap<ConstructionApplication.Core.DataModels.Site.Site, SiteAPIVm>();
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

        [HttpGet("select-site")]
        public IActionResult SelectSite(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);

            if (selectedSite == null)
                return NotFound(new { message = "Site not found" });

            return Ok(selectedSite);
        }

        [HttpPost("add")]
        public IActionResult Add(SiteAPIDTO siteApiDto)
        {
            if (siteApiDto == null)
                return BadRequest("Invalid data");

            var site = _imapper.Map<SiteAPIDTO, ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

            site.Id = _siteRepository.Create(site);

            if (site.Id <= 0)
                return StatusCode(500, "Failed to create site");

            AddAddressIfPresent(site.Id, siteApiDto);

            if (siteApiDto.SelectedMasterMasonIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.MasterMasion, siteApiDto.SelectedMasterMasonIds);
            }

            if (siteApiDto.SelectedElectricianIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Electrician, siteApiDto.SelectedElectricianIds);
            }

            if (siteApiDto.SelectedLabourIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Labour, siteApiDto.SelectedLabourIds);
            }

            if (siteApiDto.SelectedPlumberIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Plumber, siteApiDto.SelectedPlumberIds);
            }

            if (siteApiDto.SelectedPainterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Painter, siteApiDto.SelectedPainterIds);
            }

            if (siteApiDto.SelectedCarpenterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Carpenter, siteApiDto.SelectedCarpenterIds);
            }

            if (siteApiDto.SelectedTilerIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Tiler, siteApiDto.SelectedTilerIds);
            }

            return Ok(new
            {
                message = "Add New Site Successful",
                siteId = site.Id
            });
        }

        //[HttpPost("add")]
        //public IActionResult Add(SiteAPIDTO siteApiVm)
        //{
        //    if (siteApiVm == null)
        //        return BadRequest("Invalid data");

        //    var site = 
        //        _imapper.Map<SiteAPIDTO, ConstructionApplication.Core.DataModels.Site.Site>(siteApiVm);

        //    site.Id = _siteRepository.Create(site);

        //    if (site.Id <= 0)
        //        return StatusCode(500, "Failed to create site");

        //    // Address
        //    AddAddressIfPresent(site.Id, siteApiVm);

        //    // Service Providers
        //    if (siteApiVm.MasterMasonIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.MasterMasion, siteApiVm.MasterMasonIds);

        //    if (siteApiVm.ElectricianIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Electrician, siteApiVm.ElectricianIds);

        //    if (siteApiVm.LabourIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Labour, siteApiVm.LabourIds);

        //    if (siteApiVm.PlumberIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Plumber, siteApiVm.PlumberIds);

        //    if (siteApiVm.PainterIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Painter, siteApiVm.PainterIds);

        //    if (siteApiVm.CarpenterIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Carpenter, siteApiVm.CarpenterIds);

        //    if (siteApiVm.TilerIds?.Count > 0)
        //        _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //            site.Id, ServiceTypes.Tiler, siteApiVm.TilerIds);

        //    return Ok(new
        //    {
        //        message = "Add New Site Successful",
        //        siteId = site.Id
        //    });
        //}

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);

            if (selectedSite == null)
                return NotFound(new { message = "Site not found" });

            var siteApiVm = 
                _imapper.Map<ConstructionApplication.Core.DataModels.Site.Site, SiteAPIVm>(selectedSite);

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

        //[HttpPost("update")]
        //public IActionResult Update(SiteAPIDTO siteApiDto)
        //{
        //    if (siteApiDto == null)
        //        return BadRequest("Invalid data");

        //    var site = 
        //        _imapper.Map<SiteAPIVm, ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

        //    int affectedRowCount = _siteRepository.Update(site);

        //    if (affectedRowCount <= 0)
        //        return NotFound(new { message = "Site not found or update failed" });

        //    // Address update
        //    AddAddressIfPresent(site.Id, siteApiDto);

        //    //// Service Providers Update
        //    //if (siteApiDto.MasterMasonIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.MasterMasion, siteApiDto.MasterMasonIds);

        //    //if (siteApiDto.ElectricianIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Electrician, siteApiDto.ElectricianIds);

        //    //if (siteApiDto.LabourIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Labour, siteApiDto.LabourIds);

        //    //if (siteApiDto.PlumberIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Plumber, siteApiDto.PlumberIds);

        //    //if (siteApiDto.PainterIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Painter, siteApiDto.PainterIds);

        //    //if (siteApiDto.CarpenterIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Carpenter, siteApiDto.CarpenterIds);

        //    //if (siteApiDto.TilerIds?.Count > 0)
        //    //    _siteRepository.AddAndUpdateSiteServiceProviderBridge(
        //    //        site.Id, ServiceTypes.Tiler, siteApiDto.TilerIds);

        //    return Ok(new
        //    {
        //        message = "Site updated successfully",
        //        siteId = site.Id
        //    });
        //}

        [HttpDelete("{siteId}")]
        public IActionResult Delete(int siteId)
        {
            if (siteId <= 0)
                return BadRequest();

            _addressRepository.Delete(0, siteId);
            _siteRepository.Delete(siteId);

            return NoContent();
        }

        [HttpGet("dropdown-data")]
        public IActionResult GetDropdownData()
        {
            var response = new SiteDropdownDTO
            {
                Statuses = _siteStatusRepository.GetAll()
                    .Select(statuses => new DropdownItemDTO
                    {
                        Id = statuses.Id,
                        Name = statuses.Status
                    }).ToList(),

                AddressTypes = _addressTypeRepository.GetAll()
                    .Select(addressTypes => new DropdownItemDTO
                    {
                        Id = addressTypes.Id,
                        Name = addressTypes.Name
                    }).ToList(),

                Countries = _countryRepository.GetAllCountries()
                    .Select(countries => new DropdownItemDTO
                    {
                        Id = countries.Id,
                        Name = countries.Name
                    }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("service-providers")]
        public IActionResult GetServiceProviders()
        {
            var allServiceProviders = _serviceProviderRepository.GetAllServiceProviders();

            var response = new
            {
                masterMasons = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.MasterMasion)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                electricians = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Electrician)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                labours = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Labour)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                plumbers = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Plumber)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                painters = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Painter)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                carpenters = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Carpenter)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                tilers = allServiceProviders
                    .Where(serviceProvider => serviceProvider.ServiceTypeId == (int)ServiceTypes.Tiler)
                    .Select(serviceProvider => new
                    {
                        id = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList()
            };

            return Ok(response);
        }

        private void AddAddressIfPresent(int siteId, SiteAPIDTO siteApiDto)
        {
            if (siteId <= 0)
                return;

            if (!string.IsNullOrEmpty(siteApiDto.AddressLine1) ||
                (siteApiDto.AddressTypeId.HasValue && siteApiDto.AddressTypeId > 0) ||
                (siteApiDto.CountryId.HasValue && siteApiDto.CountryId > 0) ||
                (siteApiDto.PinCode.HasValue && siteApiDto.PinCode > 0))
            {
                Address address = new Address(
                    0,
                    siteApiDto.AddressLine1,
                    siteApiDto.AddressTypeId ?? 0,
                    siteApiDto.CountryId ?? 0,
                    siteApiDto.PinCode ?? 0,
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

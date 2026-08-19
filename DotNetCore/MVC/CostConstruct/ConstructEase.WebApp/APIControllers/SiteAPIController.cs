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
        private readonly ISiteStatusRepository      _siteStatusRepository;
        IAddressRepository                          _addressRepository;
        IAddressTypeRepository                      _addressTypeRepository;
        ICountryRepository                          _countryRepository;
        IServiceProviderRepository                  _serviceProviderRepository;
        ISiteRepository                             _siteRepository;
        IMapper                                     _imapper;
        IMemoryCache                                _cache;
        ISiteScopeMasterRepository                  _siteScopeMasterRepository;
        ISiteScopeRepository                        _siteScopeRepository;

        public SiteAPIController(ISiteStatusRepository siteStatusRepository,
                                 IAddressRepository addressRepository,
                                 IAddressTypeRepository addressTypeRepository,
                                 ICountryRepository countryRepository,
                                 IServiceProviderRepository serviceProviderRepository,
                                 ISiteRepository siteRepository,
                                 IMemoryCache cache,
                                 ISiteScopeMasterRepository siteScopeMasterRepository,
                                 ISiteScopeRepository siteScopeRepository)
        {
            _siteRepository            = siteRepository;
            _siteStatusRepository      = siteStatusRepository;
            _addressRepository         = addressRepository;
            _addressTypeRepository     = addressTypeRepository;
            _countryRepository         = countryRepository;
            _serviceProviderRepository = serviceProviderRepository;
            _cache                     = cache;
            _siteScopeMasterRepository = siteScopeMasterRepository;
            _siteScopeRepository       = siteScopeRepository;

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
                _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>,
                             List<SiteAPIDTO>>(sites);

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

            var site =
                _imapper.Map<SiteAPIDTO,
                             ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

            site.Id = _siteRepository.Create(site);

            if (site.Id <= 0)
                return StatusCode(500, "Failed to create site");

            AddAddressIfPresent(site.Id, siteApiDto);

            if (siteApiDto.SelectedMasterMasonIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.MasterMasion,
                    siteApiDto.SelectedMasterMasonIds);
            }

            if (siteApiDto.SelectedElectricianIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Electrician,
                    siteApiDto.SelectedElectricianIds);
            }

            if (siteApiDto.SelectedLabourIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Labour,
                    siteApiDto.SelectedLabourIds);
            }

            if (siteApiDto.SelectedPlumberIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Plumber,
                    siteApiDto.SelectedPlumberIds);
            }

            if (siteApiDto.SelectedPainterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Painter,
                    siteApiDto.SelectedPainterIds);
            }

            if (siteApiDto.SelectedCarpenterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Carpenter,
                    siteApiDto.SelectedCarpenterIds);
            }

            if (siteApiDto.SelectedTilerIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Tiler,
                    siteApiDto.SelectedTilerIds);
            }

            // NEW — Site Scopes save
            if (siteApiDto.SelectedScopes?.Count > 0)
            {
                var mappedScopes = siteApiDto.SelectedScopes
                    .Select(s => (s.SiteScopeMasterId, s.ScopeStatusId, s.Remarks))
                    .ToList();

                _siteScopeRepository.SaveScopes(site.Id, mappedScopes);
            }

            _cache.Remove("AllSites");

            return Ok(new
            {
                message = "Add New Site Successful",
                siteId  = site.Id
            });
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var selectedSite = _siteRepository.GetSiteById(id);

            if (selectedSite == null)
                return NotFound(new { message = "Site not found" });

            var siteApiVm =
                _imapper.Map<ConstructionApplication.Core.DataModels.Site.Site,
                             SiteAPIVm>(selectedSite);

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

            // NEW — Already selected scopes for this site
            var scopes = _siteScopeRepository.GetBySiteId(id);
            siteApiVm.Scopes = scopes.Select(scopes => new SiteScopeVm
            {
                Id                = scopes.Id,
                SiteScopeMasterId = scopes.SiteScopeId,
                ScopeName         = scopes.ScopeName,
                ScopeStatusId     = scopes.ScopeStatusId,
                StatusName        = scopes.StatusName,
                Remarks           = scopes.Remarks,
                CompletedDate     = scopes.CompletedDate,
            }).ToList();

            return Ok(siteApiVm);
        }

        [HttpPost("update")]
        public IActionResult Update(SiteAPIDTO siteApiDto)
        {
            if (siteApiDto == null)
                return BadRequest("Invalid data");

            var site =
                _imapper.Map<SiteAPIDTO,
                             ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

            int affectedRowCount = _siteRepository.Update(site);

            if (affectedRowCount <= 0)
                return NotFound(new { message = "Site not found or update failed" });

            AddAddressIfPresent(site.Id, siteApiDto);

            if (siteApiDto.SelectedMasterMasonIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.MasterMasion,
                    siteApiDto.SelectedMasterMasonIds);
            }

            if (siteApiDto.SelectedElectricianIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Electrician,
                    siteApiDto.SelectedElectricianIds);
            }

            if (siteApiDto.SelectedLabourIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Labour,
                    siteApiDto.SelectedLabourIds);
            }

            if (siteApiDto.SelectedPlumberIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Plumber,
                    siteApiDto.SelectedPlumberIds);
            }

            if (siteApiDto.SelectedPainterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Painter,
                    siteApiDto.SelectedPainterIds);
            }

            if (siteApiDto.SelectedCarpenterIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Carpenter,
                    siteApiDto.SelectedCarpenterIds);
            }

            if (siteApiDto.SelectedTilerIds?.Count > 0)
            {
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                    site.Id,
                    ServiceTypes.Tiler,
                    siteApiDto.SelectedTilerIds);
            }

            var mappedScopes = (siteApiDto.SelectedScopes ?? new List<ScopeSaveItem>())
                .Select(s => (s.SiteScopeMasterId, s.ScopeStatusId, s.Remarks))
                .ToList();

            _siteScopeRepository.SaveScopes(site.Id, mappedScopes);

            _cache.Remove("AllSites");

            return Ok(new { message = "Site updated successfully." });
        }

        [HttpDelete("{siteId}")]
        public IActionResult Delete(int siteId)
        {
            if (siteId <= 0)
                return BadRequest();

            _siteScopeRepository.SaveScopes(
                siteId, new List<(int, int, string?)>());

            _addressRepository.Delete(0, siteId);
            _siteRepository.Delete(siteId);

            _cache.Remove("AllSites");

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
                        Id   = statuses.Id,
                        Name = statuses.Status
                    }).ToList(),

                AddressTypes = _addressTypeRepository.GetAll()
                    .Select(addressTypes => new DropdownItemDTO
                    {
                        Id   = addressTypes.Id,
                        Name = addressTypes.Name
                    }).ToList(),

                Countries = _countryRepository.GetAllCountries()
                    .Select(countries => new DropdownItemDTO
                    {
                        Id   = countries.Id,
                        Name = countries.Name
                    }).ToList(),

                // NEW
                ScopeMasters = _siteScopeMasterRepository.GetAll()
                    .Select(s => new DropdownItemDTO
                    {
                        Id   = s.Id,
                        Name = s.ScopeName
                    }).ToList(),

                ScopeStatuses = _siteScopeRepository.GetAllStatuses()
                    .Select(s => new DropdownItemDTO
                    {
                        Id   = s.Id,
                        Name = s.StatusName
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
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.MasterMasion)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                electricians = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Electrician)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                labours = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Labour)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                plumbers = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Plumber)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                painters = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Painter)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                carpenters = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Carpenter)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList(),

                tilers = allServiceProviders
                    .Where(serviceProvider =>
                        serviceProvider.ServiceTypeId == (int)ServiceTypes.Tiler)
                    .Select(serviceProvider => new
                    {
                        id   = serviceProvider.Id,
                        name = serviceProvider.Name
                    })
                    .ToList()
            };

            return Ok(response);
        }

        [HttpPatch("scope-status")]
        public IActionResult UpdateScopeStatus([FromBody] UpdateScopeStatusDto dto)
        {
            DateTime? completedDate =
                dto.ScopeStatusId == 3 ? DateTime.Today : (DateTime?)null;

            _siteScopeRepository.UpdateScopeStatus(
                dto.SiteScopeId,
                dto.ScopeStatusId,
                dto.Remarks,
                completedDate);

            return Ok(new { message = "Scope status updated." });
        }

        private void AddAddressIfPresent(int siteId, SiteAPIDTO siteApiDto)
        {
            if (!string.IsNullOrEmpty(siteApiDto.AddressLine1)
                || siteApiDto.AddressTypeId > 0
                || siteApiDto.CountryId     > 0
                || siteApiDto.PinCode       > 0)
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
        }
    }

    public class UpdateScopeStatusDto
    {
        public int     SiteScopeId   { get; set; }
        public int     ScopeStatusId { get; set; }
        public string? Remarks       { get; set; }
    }
}
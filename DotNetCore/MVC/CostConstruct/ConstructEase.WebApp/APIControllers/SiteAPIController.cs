using AutoMapper;
using ConstructEase.WebApp.APIControllers.APIViewModels;
using ConstructEase.WebApp.Services;
using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteAPIController : ControllerBase
    {
        private readonly ISiteStatusRepository          _siteStatusRepository;
        private readonly IAddressRepository             _addressRepository;
        private readonly IAddressTypeRepository         _addressTypeRepository;
        private readonly ICountryRepository             _countryRepository;
        private readonly IServiceProviderRepository     _serviceProviderRepository;
        private readonly ISiteRepository                _siteRepository;
        private readonly IMapper                        _imapper;
        private readonly IMemoryCache                   _cache;
        private readonly ISiteScopeMasterRepository     _siteScopeMasterRepository;
        private readonly ISiteScopeRepository           _siteScopeRepository;
        private readonly ILogger<SiteAPIController>     _logger;
        private readonly IEmailService                  _emailService;

        public SiteAPIController(
            ISiteStatusRepository           siteStatusRepository,
            IAddressRepository              addressRepository,
            IAddressTypeRepository          addressTypeRepository,
            ICountryRepository              countryRepository,
            IServiceProviderRepository      serviceProviderRepository,
            ISiteRepository                 siteRepository,
            IMemoryCache                    cache,
            ISiteScopeMasterRepository      siteScopeMasterRepository,
            ISiteScopeRepository            siteScopeRepository,
            ILogger<SiteAPIController>      logger,
            IEmailService                   emailService)
        {
            _siteRepository                 = siteRepository;
            _siteStatusRepository           = siteStatusRepository;
            _addressRepository              = addressRepository;
            _addressTypeRepository          = addressTypeRepository;
            _countryRepository              = countryRepository;
            _serviceProviderRepository      = serviceProviderRepository;
            _cache = cache;
            _siteScopeMasterRepository      = siteScopeMasterRepository;
            _siteScopeRepository            = siteScopeRepository;
            _logger                         = logger;
            _emailService                   = emailService;

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
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation("API Action: GetAllSites | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));
            try
            {
                const string cacheKey = "AllSites";

                List<ConstructionApplication.Core.DataModels.Site.Site> sites;

                if (!_cache.TryGetValue(cacheKey, out sites))
                {
                    sites = ExecuteAndLog("ISiteRepository.GetAllSites",
                            () => _siteRepository.GetAllSites());

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    _cache.Set(cacheKey, sites, cacheEntryOptions);
                }

                var siteApiVm = _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>,
                                             List<SiteAPIDTO>>(sites);

                _logger.LogInformation("\nTotal Site: {TotalSite}", sites?.Count ?? 0);

                _logger.LogInformation("\nError: None | Exception: None");

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("GetAllSites", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation("\nAPI Action: GetAllSites | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        [HttpGet("filter")]
        public IActionResult FilterSites(string? search, int? statusId, DateTime? fromDate,
                                         DateTime? toDate, decimal? budgetFrom, decimal? budgetTo)
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation("API Action: FilterSites | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                {
                    _logger.LogInformation("\nError: From Date cannot be greater than To Date | Exception: None");

                    return BadRequest(new
                    {
                        message = "From Date cannot be greater than To Date."
                    });
                }

                if (budgetFrom.HasValue && budgetFrom < 0)
                {
                    _logger.LogInformation("\nError: Budget From cannot be negative | Exception: None");

                    return BadRequest(new
                    {
                        message = "Budget From cannot be negative."
                    });
                }

                if (budgetTo.HasValue && budgetTo < 0)
                {
                    _logger.LogInformation("\nError: Budget To cannot be negative | Exception: None");

                    return BadRequest(new
                    {
                        message = "Budget To cannot be negative."
                    });
                }

                if (budgetFrom.HasValue && budgetTo.HasValue && budgetFrom > budgetTo)
                {
                    _logger.LogInformation("\nError: Budget From cannot be greater than Budget To | Exception: None");

                    return BadRequest(new
                    {
                        message = "Budget From cannot be greater than Budget To."
                    });
                }

                var sites = ExecuteAndLog("ISiteRepository.GetSites",
                    () => _siteRepository.GetSites(search, statusId, fromDate,
                                                    toDate, budgetFrom, budgetTo));
                var siteApiVm = _imapper.Map<List<ConstructionApplication.Core.DataModels.Site.Site>,
                                             List<SiteAPIDTO>>(sites);

                _logger.LogInformation("\nTotal Site: {TotalSite}", sites?.Count ?? 0);

                _logger.LogInformation("\nError: None | Exception: None");

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("FilterSites", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation("\nAPI Action: FilterSites | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        [HttpGet("select-site")]
        public IActionResult SelectSite(int id)
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation("API Action: SelectSite | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));
            try
            {
                var selectedSite = ExecuteAndLog("ISiteRepository.GetSiteById",
                    () => _siteRepository.GetSiteById(id));

                if (selectedSite == null)
                {
                    _logger.LogInformation("\nError: Site not found | Exception: None");

                    return NotFound(new
                    {
                        message = "Site not found"
                    });
                }

                _logger.LogInformation("\nError: None | Exception: None");

                return Ok(selectedSite);
            }
            catch (Exception ex)
            {
                LogException("SelectSite", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation("\nAPI Action: SelectSite | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(SiteAPIDTO siteApiDto)
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation("API Action: Add | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteApiDto == null)
                {
                    _logger.LogInformation("\nError: Invalid data | Exception: None");

                    return BadRequest("Invalid data");
                }

                var site = _imapper.Map<
                    SiteAPIDTO,
                    ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

                site.Id = ExecuteAndLog("ISiteRepository.Create", () => _siteRepository.Create(site));

                if (site.Id <= 0)
                {
                    _logger.LogInformation("\nError: Failed to create site | Exception: None");

                    return StatusCode(500, "Failed to create site");
                }

                ExecuteAndLog("AddAddressIfPresent", () => AddAddressIfPresent(site.Id, siteApiDto));

                if (siteApiDto.SelectedMasterMasonIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.MasterMasion,
                            siteApiDto.SelectedMasterMasonIds));
                }

                if (siteApiDto.SelectedElectricianIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Electrician,
                            siteApiDto.SelectedElectricianIds));
                }

                if (siteApiDto.SelectedLabourIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Labour,
                            siteApiDto.SelectedLabourIds));
                }

                if (siteApiDto.SelectedPlumberIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Plumber,
                            siteApiDto.SelectedPlumberIds));
                }

                if (siteApiDto.SelectedPainterIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Painter,
                            siteApiDto.SelectedPainterIds));
                }

                if (siteApiDto.SelectedCarpenterIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Carpenter,
                            siteApiDto.SelectedCarpenterIds));
                }

                if (siteApiDto.SelectedTilerIds?.Count > 0)
                {
                    ExecuteAndLog("ISiteRepository.AddAndUpdateSiteServiceProviderBridge",
                        () => _siteRepository.AddAndUpdateSiteServiceProviderBridge(
                            site.Id,
                            ServiceTypes.Tiler,
                            siteApiDto.SelectedTilerIds));
                }

                if (siteApiDto.SelectedScopes?.Count > 0)
                {
                    var mappedScopes = siteApiDto.SelectedScopes
                        .Select(s => (s.SiteScopeMasterId, s.ScopeStatusId, s.Remarks)).ToList();

                    ExecuteAndLog("ISiteScopeRepository.SaveScopes",
                        () => _siteScopeRepository.SaveScopes(site.Id, mappedScopes));
                }

                _cache.Remove("AllSites");

                // Get the actual Site Status name from the database.
                var siteStatuses = ExecuteAndLog("ISiteStatusRepository.GetAll", () => _siteStatusRepository.GetAll());

                var siteStatus = siteStatuses.FirstOrDefault(x => x.Id == site.SiteStatusId);

                string siteStatusName = siteStatus?.Status ?? "Not Provided";

                // Build site address for email.
                string siteAddress = string.Join(", ", new[] {
                       siteApiDto.AddressLine1,
                       siteApiDto.CountryName,
                       siteApiDto.PinCode?.ToString()
                       }
                       .Where(x => !string.IsNullOrWhiteSpace(x)));

                // Get selected work scopes for email.
                var siteScopes = ExecuteAndLog("ISiteScopeRepository.GetBySiteId", () => _siteScopeRepository.GetBySiteId(site.Id));

                string workScope = string.Join(" · ",
                    siteScopes.Select(x => x.ScopeName)
                              .Where(x => !string.IsNullOrWhiteSpace(x))
                              .Distinct());

                // Get logged-in user's name.
                string addedByName = User?.Identity?.Name ?? "System";

                // Send email BEFORE returning successful response.
                try
                {
                    await _emailService.SendNewSiteNotificationAsync(
                          site.Id,
                          site.Name ?? "Not Provided",
                          site.ContactName ?? "Not Provided",
                          site.ContactNumber ?? "Not Provided",
                          siteStatusName,
                          site.ExpectedBudget,
                          site.StartedDate,
                          site.ExpectedCompletionDate,
                          siteAddress,
                          workScope,
                          site.Note,
                          addedByName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Site created but email notification failed | SiteId: {SiteId}",site.Id);

                    return Ok(new
                    {
                        message = "Site was successfully saved, but the notification email could not be sent.",
                        siteId = site.Id,
                        emailSent = false
                    });
                }

                _logger.LogInformation("Success: Add New Site Successful | SiteId: {SiteId} | Email Sent: True", site.Id);

                return Ok(new
                {
                    message = "Site was successfully saved and the notification email was sent.",
                    siteId = site.Id,
                    emailSent = true
                });
            }
            catch (Exception ex)
            {
                LogException("Add", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation("\nAPI Action: Add | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("API Action: Edit | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                ConstructionApplication.Core.DataModels.Site.Site selectedSite = ExecuteAndLog(
                    "ISiteRepository.GetSiteById", () => _siteRepository.GetSiteById(id));

                _siteRepository.GetSiteById(id);
                
                
                if (selectedSite == null)
                {
                    _logger.LogInformation(
                        "\nError: Site not found | Exception: None");

                    return NotFound(new
                    {
                        message = "Site not found"
                    });
                }

                var siteApiVm =_imapper.Map< ConstructionApplication.Core.DataModels.Site.Site, SiteAPIVm>(selectedSite);

                siteApiVm.MasterMasonIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.MasterMasion
                        }));

                siteApiVm.ElectricianIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Electrician
                        }));

                siteApiVm.LabourIds = ExecuteAndLog("ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Labour
                        }));

                siteApiVm.PlumberIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Plumber
                        }));

                siteApiVm.PainterIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Painter
                        }));

                siteApiVm.CarpenterIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Carpenter
                        }));

                siteApiVm.TilerIds = ExecuteAndLog(
                    "ISiteRepository.GetServiceProviderIdsByTypes",
                    () => _siteRepository.GetServiceProviderIdsByTypes(
                        id,
                        new List<ServiceTypes>
                        {
                            ServiceTypes.Tiler
                        }));

                var scopes = ExecuteAndLog(
                    "ISiteScopeRepository.GetBySiteId",
                    () => _siteScopeRepository.GetBySiteId(id));

                siteApiVm.Scopes = scopes
                    .Select(scopes => new SiteScopeVm
                    {
                        Id = scopes.Id,
                        SiteScopeMasterId = scopes.SiteScopeId,
                        ScopeName = scopes.ScopeName,
                        ScopeStatusId = scopes.ScopeStatusId,
                        StatusName = scopes.StatusName,
                        Remarks = scopes.Remarks,
                        CompletedDate = scopes.CompletedDate,
                    })
                    .ToList();

                _logger.LogInformation(
                    "\nError: None | Exception: None");

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("Edit", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("API Action: Edit | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        // ============================================================
        // UPDATE SITE
        // ============================================================

        [HttpPost("update")]
        public IActionResult Update(SiteAPIDTO siteApiDto)
        {
            
            _logger.LogInformation("API Action: Update | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteApiDto == null)
                {
                    _logger.LogInformation("\nError: Invalid data | Exception: None");

                    return BadRequest("Invalid data");
                }

                var site =_imapper.Map<SiteAPIDTO,ConstructionApplication.Core.DataModels.Site.Site>(siteApiDto);

                int affectedRowCount = _siteRepository.Update(site);

                if (affectedRowCount <= 0)
                {
                    return NotFound("Site not found or update failed");
                }
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

                var mappedScopes = (siteApiDto.SelectedScopes ?? new List<ScopeSaveItem>())
                    .Select(s => (s.SiteScopeMasterId, s.ScopeStatusId, s.Remarks)).ToList();

                _siteScopeRepository.SaveScopes(site.Id, mappedScopes);

                _cache.Remove("AllSites");

                _logger.LogInformation("\nSuccess: Site updated successfully | Last Updated ID: {SiteId}", site.Id);

                _logger.LogInformation("\nError: None | Exception: None");

                return Ok("Site updated successfully.");
            }
            catch (Exception ex)
            {
                LogException("Update", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("API Action: Update | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        // ============================================================
        // DELETE SITE
        // ============================================================

        [HttpDelete("{siteId}")]
        public IActionResult Delete(int siteId)
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation(
                "API Action: Delete | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteId <= 0)
                {
                    _logger.LogInformation(
                        "\nError: Invalid Site ID | Exception: None");

                    return BadRequest();
                }

                ExecuteAndLog(
                    "ISiteScopeRepository.SaveScopes",
                    () => _siteScopeRepository.SaveScopes(
                        siteId,
                        new List<(int, int, string?)>()));

                ExecuteAndLog(
                    "IAddressRepository.Delete",
                    () => _addressRepository.Delete(
                        0,
                        siteId));

                ExecuteAndLog(
                    "ISiteRepository.Delete",
                    () => _siteRepository.Delete(siteId));

                _cache.Remove("AllSites");

                _logger.LogInformation(
                    "\nSuccess: Site deleted successfully | Deleted ID: {SiteId}",
                    siteId);

                _logger.LogInformation(
                    "\nError: None | Exception: None");

                return NoContent();
            }
            catch (Exception ex)
            {
                LogException("Delete", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation(
                    "\nAPI Action: Delete | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // GET DROPDOWN DATA
        // ============================================================

        [HttpGet("dropdown-data")]
        public IActionResult GetDropdownData()
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation(
                "API Action: GetDropdownData | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                var statuses = ExecuteAndLog(
                    "ISiteStatusRepository.GetAll",
                    () => _siteStatusRepository.GetAll());

                var addressTypes = ExecuteAndLog(
                    "IAddressTypeRepository.GetAll",
                    () => _addressTypeRepository.GetAll());

                var countries = ExecuteAndLog(
                    "ICountryRepository.GetAllCountries",
                    () => _countryRepository.GetAllCountries());

                var scopeMasters = ExecuteAndLog(
                    "ISiteScopeMasterRepository.GetAll",
                    () => _siteScopeMasterRepository.GetAll());

                var scopeStatuses = ExecuteAndLog(
                    "ISiteScopeRepository.GetAllStatuses",
                    () => _siteScopeRepository.GetAllStatuses());

                var response = new SiteDropdownDTO
                {
                    Statuses = statuses
                        .Select(status => new DropdownItemDTO
                        {
                            Id = status.Id,
                            Name = status.Status
                        })
                        .ToList(),

                    AddressTypes = addressTypes
                        .Select(addressType => new DropdownItemDTO
                        {
                            Id = addressType.Id,
                            Name = addressType.Name
                        })
                        .ToList(),

                    Countries = countries
                        .Select(country => new DropdownItemDTO
                        {
                            Id = country.Id,
                            Name = country.Name
                        })
                        .ToList(),

                    ScopeMasters = scopeMasters
                        .Select(scope => new DropdownItemDTO
                        {
                            Id = scope.Id,
                            Name = scope.ScopeName
                        })
                        .ToList(),

                    ScopeStatuses = scopeStatuses
                        .Select(scopeStatus => new DropdownItemDTO
                        {
                            Id = scopeStatus.Id,
                            Name = scopeStatus.StatusName
                        })
                        .ToList()
                };

                _logger.LogInformation(
                    "\nError: None | Exception: None");

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogException("GetDropdownData", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation(
                    "\nAPI Action: GetDropdownData | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // GET SERVICE PROVIDERS
        // ============================================================

        [HttpGet("service-providers")]
        public IActionResult GetServiceProviders()
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation(
                "API Action: GetServiceProviders | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                var allServiceProviders = ExecuteAndLog(
                    "IServiceProviderRepository.GetAllServiceProviders",
                    () => _serviceProviderRepository.GetAllServiceProviders());

                var response = new
                {
                    masterMasons = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.MasterMasion)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    electricians = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Electrician)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    labours = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Labour)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    plumbers = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Plumber)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    painters = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Painter)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    carpenters = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Carpenter)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList(),

                    tilers = allServiceProviders
                        .Where(serviceProvider =>
                            serviceProvider.ServiceTypeId ==
                            (int)ServiceTypes.Tiler)
                        .Select(serviceProvider => new
                        {
                            id = serviceProvider.Id,
                            name = serviceProvider.Name
                        })
                        .ToList()
                };

                _logger.LogInformation(
                    "\nError: None | Exception: None");

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogException("GetServiceProviders", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation(
                    "\nAPI Action: GetServiceProviders | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // UPDATE SCOPE STATUS
        // ============================================================

        [HttpPatch("scope-status")]
        public IActionResult UpdateScopeStatus(
            [FromBody] UpdateScopeStatusDto dto)
        {
            Stopwatch actionTimer = Stopwatch.StartNew();

            _logger.LogInformation(
                "API Action: UpdateScopeStatus | Invoked: {InvokedAt}",
                DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                DateTime? completedDate =
                    dto.ScopeStatusId == 3
                        ? DateTime.Today
                        : (DateTime?)null;

                ExecuteAndLog(
                    "ISiteScopeRepository.UpdateScopeStatus",
                    () => _siteScopeRepository.UpdateScopeStatus(
                        dto.SiteScopeId,
                        dto.ScopeStatusId,
                        dto.Remarks,
                        completedDate));

                _logger.LogInformation(
                    "\nSuccess: Scope status updated | Updated ID: {SiteScopeId}",
                    dto.SiteScopeId);

                _logger.LogInformation(
                    "\nError: None | Exception: None");

                return Ok(new
                {
                    message = "Scope status updated."
                });
            }
            catch (Exception ex)
            {
                LogException("UpdateScopeStatus", ex);
                throw;
            }
            finally
            {
                actionTimer.Stop();

                _logger.LogInformation(
                    "\nAPI Action: UpdateScopeStatus | Completed | Total Time: {ElapsedMs} ms",
                    actionTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // PRIVATE METHOD
        // ============================================================

        private void AddAddressIfPresent(
            int siteId,
            SiteAPIDTO siteApiDto)
        {
            if (!string.IsNullOrEmpty(siteApiDto.AddressLine1)
                || siteApiDto.AddressTypeId > 0
                || siteApiDto.CountryId > 0
                || siteApiDto.PinCode > 0)
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

        // ============================================================
        // METHOD TIMING HELPER - RETURN VALUE
        // ============================================================

        private T ExecuteAndLog<T>(
            string methodName,
            Func<T> action)
        {
            Stopwatch methodTimer = Stopwatch.StartNew();

            try
            {
                return action();
            }
            finally
            {
                methodTimer.Stop();

                _logger.LogInformation(
                    "\nMethod: {Method} | Time: {ElapsedMs} ms",
                    methodName,
                    methodTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // METHOD TIMING HELPER - VOID METHOD
        // ============================================================

        private void ExecuteAndLog(
            string methodName,
            Action action)
        {
            Stopwatch methodTimer = Stopwatch.StartNew();

            try
            {
                action();
            }
            finally
            {
                methodTimer.Stop();

                _logger.LogInformation(
                    "\nMethod: {Method} | Time: {ElapsedMs} ms",
                    methodName,
                    methodTimer.ElapsedMilliseconds);
            }
        }

        // ============================================================
        // EXCEPTION LOGGING
        // ============================================================

        private void LogException(string actionName, Exception ex)
        {
            _logger.LogError(ex, "\nError: API Action {Action} failed | Exception: {ExceptionType}: {ExceptionMessage}",
                             actionName,
                             ex.GetType().Name,
                             ex.Message);
        }
    }

    public class UpdateScopeStatusDto
    {
        public int SiteScopeId { get; set; }

        public int ScopeStatusId { get; set; }

        public string? Remarks { get; set; }
    }
}
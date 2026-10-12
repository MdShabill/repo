using AutoMapper;
using ConstructEase.WebApp.APIControllers.APIViewModels;
using ConstructEase.WebApp.Services;
using ConstructionApplication.Core.DataModels;
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
        private readonly UpdateEmailService             _updateEmailService;

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
            IEmailService                   emailService,
            UpdateEmailService              updateEmailService)
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
            _updateEmailService             = updateEmailService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Site, SiteAPIDTO>();

                cfg.CreateMap<SiteAPIDTO, Site>();

                cfg.CreateMap<Site, SiteAPIVm>();
            });
            _imapper = configuration.CreateMapper();
        }

        [HttpGet("GetAllSites")]
        public IActionResult GetAllSites()
        {
            _logger.LogInformation("API Action: GetAllSites | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));
            try
            {
                const string cacheKey = "AllSites";

                List<Site> sites;

                if (!_cache.TryGetValue(cacheKey, out sites))
                {
                    sites = _siteRepository.GetAllSites();

                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    _cache.Set(cacheKey, sites, cacheEntryOptions);
                }

                List<SiteAPIDTO> siteApiVm = _imapper.Map<List<Site>,List<SiteAPIDTO>>(sites);

                _logger.LogInformation("\nTotal Site: {TotalSite}", sites?.Count ?? 0);

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("GetAllSites", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: GetAllSites | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpGet("filter")]
        public IActionResult FilterSites(string? search, int? statusId, DateTime? fromDate,
                                         DateTime? toDate, decimal? budgetFrom, decimal? budgetTo)
        {
            _logger.LogInformation("API Action: FilterSites | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                {
                    return BadRequest("From Date cannot be greater than To Date.");
                }

                if (budgetFrom.HasValue && budgetFrom < 0)
                {
                    return BadRequest("Budget From cannot be negative.");
                }

                if (budgetTo.HasValue && budgetTo < 0)
                {
                    return BadRequest("Budget To cannot be negative.");
                }

                if (budgetFrom.HasValue && budgetTo.HasValue && budgetFrom > budgetTo)
                {
                    _logger.LogInformation("\nError: Budget From cannot be greater than Budget To | Exception: None");

                    return BadRequest(new
                    {
                        message = "Budget From cannot be greater than Budget To."
                    });
                }

                List<Site> sites = _siteRepository.GetSites(search, statusId, fromDate, toDate, budgetFrom, budgetTo);

                List<SiteAPIDTO> siteApiVm = _imapper.Map<List<Site>, List<SiteAPIDTO>>(sites);

                _logger.LogInformation("\nTotal Site: {TotalSite}", sites?.Count ?? 0);

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("FilterSites", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: FilterSites | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpGet("select-site")]
        public IActionResult SelectSite(int id)
        {
            _logger.LogInformation("API Action: SelectSite | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));
            try
            {
                Site selectedSite = _siteRepository.GetSiteById(id);

                if (selectedSite == null)
                {
                    return NotFound("Site not found");
                }
                return Ok(selectedSite);
            }
            catch (Exception ex)
            {
                LogException("SelectSite", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: SelectSite | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(SiteAPIDTO siteApiDto)
        {
            _logger.LogInformation("API Action: Add | Started: {StartedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteApiDto == null)
                {
                    return BadRequest("Invalid data");
                }

                Site site = _imapper.Map<SiteAPIDTO,Site>(siteApiDto);

                site.Id = _siteRepository.Create(site);

                if (site.Id <= 0)
                {
                    return StatusCode(500, "Failed to create site");
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

                // Save selected work scopes
                if (siteApiDto.SelectedScopes?.Count > 0)
                {
                    List<(int, int, string?)> mappedScopes = new List<(int, int, string?)>();

                    foreach (ScopeSaveItem scope in siteApiDto.SelectedScopes)
                    {
                        mappedScopes.Add((scope.SiteScopeMasterId, scope.ScopeStatusId, scope.Remarks));
                    }

                    _siteScopeRepository.SaveScopes(site.Id, mappedScopes);
                }


                string siteStatusName = "Not Provided";

                List<SiteStatus> siteStatuses = _siteStatusRepository.GetAll();

                foreach (var status in siteStatuses)
                {
                    if (status.Id == site.SiteStatusId)
                    {
                        siteStatusName = status.Status ?? "Not Provided";
                        break;
                    }
                }

                // Prepare site address for email

                string siteAddress = "";

                if (!string.IsNullOrWhiteSpace(siteApiDto.AddressLine1))
                    siteAddress = siteApiDto.AddressLine1;

                if (!string.IsNullOrWhiteSpace(siteApiDto.CountryName))
                {
                    if (siteAddress != "")
                        siteAddress += ", ";

                    siteAddress += siteApiDto.CountryName;
                }

                if (siteApiDto.PinCode.HasValue)
                {
                    if (siteAddress != "")
                        siteAddress += ", ";

                    siteAddress += siteApiDto.PinCode.Value.ToString();
                }

                // Prepare work scopes for email
                List<SiteScope> siteScopes = _siteScopeRepository.GetBySiteId(site.Id);
                string workScope = "";

                foreach (SiteScope scope in siteScopes)
                {
                    if (string.IsNullOrWhiteSpace(scope.ScopeName))
                        continue;

                    if (workScope.Contains(scope.ScopeName))
                        continue;

                    if (workScope != "")
                        workScope += " · ";

                    workScope += scope.ScopeName;
                }

                // Get logged-in user's name
                string addedByName = User?.Identity?.Name ?? "System";

                // Send email notification
                try
                {
                    await _emailService.SendNewSiteNotificationAsync(
                        site.Id, site.Name ?? "Not Provided", site.ContactName ?? "Not Provided", site.ContactNumber ?? "Not Provided",
                        siteStatusName, site.ExpectedBudget, site.StartedDate, site.ExpectedCompletionDate, siteAddress,
                        workScope, site.Note, addedByName);

                    return Ok(new
                    {
                        message = "Site was successfully saved and the notification email was sent.",
                        siteId = site.Id,
                        emailSent = true
                    });
                }
                catch (Exception)
                {
                    return Ok(new
                    {
                        message = "Site was successfully saved, but the notification email could not be sent.",
                        siteId = site.Id,
                        emailSent = false
                    });
                }
            }
            catch (Exception ex)
            {
                LogException("Add", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: Add | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("API Action: Edit | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                Site selectedSite = _siteRepository.GetSiteById(id);

                if (selectedSite == null)
                    return NotFound("Site not found");

                SiteAPIVm siteApiVm =_imapper.Map< Site, SiteAPIVm>(selectedSite);

                siteApiVm.MasterMasonIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.MasterMasion});

                siteApiVm.ElectricianIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Electrician});

                siteApiVm.LabourIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Labour});

                siteApiVm.PlumberIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Plumber});

                siteApiVm.PainterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Painter});

                siteApiVm.CarpenterIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Carpenter});

                siteApiVm.TilerIds = _siteRepository.GetServiceProviderIdsByTypes(id, new List<ServiceTypes>{ServiceTypes.Tiler});


                List<SiteScope> scopes = _siteScopeRepository.GetBySiteId(id);

                siteApiVm.Scopes = new List<SiteScopeVm>();

                foreach (SiteScope scope in scopes)
                {
                    SiteScopeVm scopeVm = new SiteScopeVm
                    {
                        Id = scope.Id,
                        SiteScopeMasterId = scope.SiteScopeId,
                        ScopeName = scope.ScopeName,
                        ScopeStatusId = scope.ScopeStatusId,
                        StatusName = scope.StatusName,
                        Remarks = scope.Remarks,
                        CompletedDate = scope.CompletedDate
                    };

                    siteApiVm.Scopes.Add(scopeVm);
                }

                return Ok(siteApiVm);
            }
            catch (Exception ex)
            {
                LogException("Edit", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: Edit | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] SiteAPIDTO siteApiDto)
        {
            _logger.LogInformation("API Action: Update | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteApiDto == null || siteApiDto.Id <= 0)
                    return BadRequest("Invalid site data.");

                // 1. Get original database values before updating.
                Site oldSite = _siteRepository.GetSiteById(siteApiDto.Id);

                if (oldSite == null)
                    return NotFound("Site not found.");

                List<SiteScope> oldScopes = _siteScopeRepository.GetBySiteId(siteApiDto.Id);
                List<SiteStatus> allSiteStatuses = _siteStatusRepository.GetAll();
                List<SiteScopeStatus> allScopeStatuses = _siteScopeRepository.GetAllStatuses();
                List<SiteScopeMaster> allScopeMasters = _siteScopeMasterRepository.GetAll();

                List<SiteUpdateChange> changes = new List<SiteUpdateChange>();
                List<ScopeSaveItem> newScopes = siteApiDto.SelectedScopes ?? new();

                // 2. Check Site Status.
                if (oldSite.SiteStatusId != siteApiDto.SiteStatusId)
                {
                    string oldStatus = "Not Provided";
                    string newStatus = "Not Provided";

                    foreach (var status in allSiteStatuses)
                    {
                        if (status.Id == oldSite.SiteStatusId)
                            oldStatus = status.Status ?? "Not Provided";

                        if (status.Id == siteApiDto.SiteStatusId)
                            newStatus = status.Status ?? "Not Provided";
                    }

                    changes.Add(new SiteUpdateChange
                    {
                        FieldName = "Site Status",
                        OldValue = oldStatus,
                        NewValue = newStatus
                    });
                }

                // 3. Check Expected Budget.
                if (oldSite.ExpectedBudget != siteApiDto.ExpectedBudget)
                {
                    changes.Add(new SiteUpdateChange
                    {
                        FieldName = "Expected Budget", OldValue = oldSite.ExpectedBudget.HasValue
                            ? $"₹{oldSite.ExpectedBudget.Value:N0}" : "Not Provided",

                        NewValue = siteApiDto.ExpectedBudget.HasValue ? $"₹{siteApiDto.ExpectedBudget.Value:N0}" : "Not Provided"
                    });
                }

                // 4. Check removed scopes and changed scope statuses.
                foreach (SiteScope oldScope in oldScopes)
                {
                    ScopeSaveItem? matchingScope = null;

                    foreach (ScopeSaveItem newScope in newScopes)
                    {
                        if (newScope.SiteScopeMasterId == oldScope.SiteScopeId)
                        {
                            matchingScope = newScope;
                            break;
                        }
                    }

                    string scopeName = oldScope.ScopeName ?? $"Scope #{oldScope.SiteScopeId}";

                    // Scope removed.
                    if (matchingScope == null)
                    {
                        changes.Add(new SiteUpdateChange
                        {
                            FieldName = "Work Scope Removed",
                            OldValue = scopeName,
                            NewValue = "Removed"
                        });

                        continue;
                    }

                    // Scope status changed.
                    if (oldScope.ScopeStatusId != matchingScope.ScopeStatusId)
                    {
                        string oldStatus = oldScope.StatusName ?? "Not Provided";
                        string newStatus = "Not Provided";

                        foreach (SiteScopeStatus status in allScopeStatuses)
                        {
                            if (status.Id == matchingScope.ScopeStatusId)
                            {
                                newStatus = status.StatusName ?? "Not Provided";
                                break;
                            }
                        }

                        changes.Add(new SiteUpdateChange
                        {
                            FieldName = $"Work Scope Status - {scopeName}",
                            OldValue = oldStatus,
                            NewValue = newStatus
                        });
                    }
                }

                // 5. Check newly added scopes.
                foreach (ScopeSaveItem newScope in newScopes)
                {
                    bool alreadyExists = false;

                    foreach (SiteScope oldScope in oldScopes)
                    {
                        if (oldScope.SiteScopeId == newScope.SiteScopeMasterId)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (!alreadyExists)
                    {
                        string scopeName = $"Scope #{newScope.SiteScopeMasterId}";

                        foreach (SiteScopeMaster master in allScopeMasters)
                        {
                            if (master.Id == newScope.SiteScopeMasterId)
                            {
                                scopeName = master.ScopeName ?? scopeName;
                                break;
                            }
                        }

                        changes.Add(new SiteUpdateChange
                        {
                            FieldName = "Work Scope Added",
                            OldValue = "Not Assigned",
                            NewValue = scopeName
                        });
                    }
                }

                Site site = _imapper.Map<SiteAPIDTO,Site>(siteApiDto);

                int affectedRows = _siteRepository.Update(site);

                if (affectedRows <= 0)
                    return StatusCode(500, "Site update failed.");

                // 7. Save address.
                AddAddressIfPresent(site.Id, siteApiDto);

                // 8. Save service provider assignments. These changes do NOT trigger an email.
                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.MasterMasion, siteApiDto.SelectedMasterMasonIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Electrician, siteApiDto.SelectedElectricianIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Labour, siteApiDto.SelectedLabourIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Plumber, siteApiDto.SelectedPlumberIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Painter, siteApiDto.SelectedPainterIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Carpenter, siteApiDto.SelectedCarpenterIds ?? new List<int>());

                _siteRepository.AddAndUpdateSiteServiceProviderBridge(site.Id, ServiceTypes.Tiler, siteApiDto.SelectedTilerIds ?? new List<int>());

                // 9. Save selected work scopes.
                List<(int, int, string?)> mappedScopes = new List<(int, int, string?)>();

                foreach (ScopeSaveItem scope in newScopes)
                {
                    mappedScopes.Add((scope.SiteScopeMasterId, scope.ScopeStatusId, scope.Remarks));
                }

                _siteScopeRepository.SaveScopes(site.Id, mappedScopes);

                // Refresh cached sites.
                _cache.Remove("AllSites");

                // 10. Send one email only when an important change exists.
                bool emailSent = false;

                if (changes.Count > 0)
                {
                    try
                    {
                        await _updateEmailService.SendAsync(site.Name ?? oldSite.Name ?? "Unnamed Site", 
                                                            User?.Identity?.Name ?? "System", DateTime.Now, changes);
                        emailSent = true;
                    }
                    catch (Exception emailException)
                    {
                        _logger.LogError(emailException, "Site {SiteId} updated, but notification email failed.", site.Id);
                    }
                }

                _logger.LogInformation("Site {SiteId} updated. Important changes: {ChangeCount}. Email sent: {EmailSent}", site.Id, changes.Count, emailSent);

                return Ok(new
                {
                    message = "Site updated successfully.",
                    siteId = site.Id,
                    importantChanges = changes.Count,
                    emailSent
                });
            }
            catch (Exception ex)
            {
                LogException("Update", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("API Action: Update " + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpDelete("{siteId}")]
        public IActionResult Delete(int siteId)
        {
            _logger.LogInformation("API Action: Delete | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (siteId <= 0)
                {
                    return BadRequest();
                }

                _siteScopeRepository.SaveScopes(siteId, new List<(int, int, string?)>());

                _addressRepository.Delete(0,siteId);

                _siteRepository.Delete(siteId);

                return NoContent();
            }
            catch (Exception ex)
            {
                LogException("Delete", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation(
                    "\nAPI Action: Delete | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }


        [HttpGet("dropdown-data")]
        public IActionResult GetDropdownData()
        {
            _logger.LogInformation("API Action: GetDropdownData | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                List<SiteStatus> statuses = _siteStatusRepository.GetAll();
                List<AddressType> addressTypes = _addressTypeRepository.GetAll();
                List<Country> countries = _countryRepository.GetAllCountries();
                List<SiteScopeMaster> scopeMasters = _siteScopeMasterRepository.GetAll();
                List<SiteScopeStatus> scopeStatuses = _siteScopeRepository.GetAllStatuses();

                // Site Statuses
                List<DropdownItemDTO> statusItems = new List<DropdownItemDTO>();

                foreach (SiteStatus status in statuses)
                {
                    statusItems.Add(new DropdownItemDTO
                    {
                        Id = status.Id,
                        Name = status.Status
                    });
                }

                // Address Types
                List<DropdownItemDTO> addressTypeItems = new List<DropdownItemDTO>();

                foreach (AddressType addressType in addressTypes)
                {
                    addressTypeItems.Add(new DropdownItemDTO
                    {
                        Id = addressType.Id,
                        Name = addressType.Name
                    });
                }

                // Countries
                List<DropdownItemDTO> countryItems = new List<DropdownItemDTO>();

                foreach (Country country in countries)
                {
                    countryItems.Add(new DropdownItemDTO
                    {
                        Id = country.Id,
                        Name = country.Name
                    });
                }

                // Work Scope Masters
                List<DropdownItemDTO> scopeMasterItems = new List<DropdownItemDTO>();

                foreach (SiteScopeMaster scope in scopeMasters)
                {
                    scopeMasterItems.Add(new DropdownItemDTO
                    {
                        Id = scope.Id,
                        Name = scope.ScopeName
                    });
                }

                // Work Scope Statuses
                List<DropdownItemDTO> scopeStatusItems = new List<DropdownItemDTO>();

                foreach (SiteScopeStatus scopeStatus in scopeStatuses)
                {
                    scopeStatusItems.Add(new DropdownItemDTO
                    {
                        Id = scopeStatus.Id,
                        Name = scopeStatus.StatusName
                    });
                }

                SiteDropdownDTO response = new SiteDropdownDTO
                {
                    Statuses = statusItems, AddressTypes = addressTypeItems, Countries = countryItems,
                    ScopeMasters = scopeMasterItems, ScopeStatuses = scopeStatusItems
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogException("GetDropdownData", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("API Action: GetDropdownData" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpGet("service-providers")]
        public IActionResult GetServiceProviders()
        {
            _logger.LogInformation("API Action: GetServiceProviders | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                List<ServiceProviderName> allServiceProviders =_serviceProviderRepository.GetAllServiceProviders();

                List<object> masterMasons = new List<object>();
                List<object> electricians = new List<object>();
                List<object> labours = new List<object>();
                List<object> plumbers = new List<object>();
                List<object> painters = new List<object>();
                List<object> carpenters = new List<object>();
                List<object> tilers = new List<object>();

                foreach (ServiceProviderName serviceProvider in allServiceProviders)
                {
                    if (serviceProvider.ServiceTypeId == (int)ServiceTypes.MasterMasion)
                    {
                        masterMasons.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Electrician)
                    {
                        electricians.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Labour)
                    {
                        labours.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Plumber)
                    {
                        plumbers.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Painter)
                    {
                        painters.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Carpenter)
                    {
                        carpenters.Add(new { id = serviceProvider.Id,name = serviceProvider.Name});
                    }
                    else if (serviceProvider.ServiceTypeId == (int)ServiceTypes.Tiler)
                    {
                        tilers.Add(new { id = serviceProvider.Id, name = serviceProvider.Name});
                    }
                }

                var response = new{masterMasons, electricians, labours, plumbers, painters, carpenters, tilers};

                return Ok(response);
            }
            catch (Exception ex)
            {
                LogException("GetServiceProviders", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("API Action: GetServiceProviders" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        [HttpPatch("scope-status")]
        public IActionResult UpdateScopeStatus([FromBody] UpdateScopeStatusDto dto)
        {
            _logger.LogInformation("API Action: UpdateScopeStatus | Invoked: {InvokedAt}", DateTime.Now.ToString("HH:mm:ss.fff"));

            try
            {
                if (dto == null || dto.SiteScopeId <= 0 || dto.ScopeStatusId <= 0)
                    return BadRequest("Invalid scope status data.");

                DateTime? completedDate = null;

                if (dto.ScopeStatusId == 3)
                    completedDate = DateTime.Today;

                _siteScopeRepository.UpdateScopeStatus(dto.SiteScopeId, dto.ScopeStatusId, dto.Remarks, completedDate);

                return Ok("Scope status updated.");
            }
            catch (Exception ex)
            {
                LogException("UpdateScopeStatus", ex);
                throw;
            }
            finally
            {
                _logger.LogInformation("\nAPI Action: UpdateScopeStatus | Completed" + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }

        private void AddAddressIfPresent(int siteId, SiteAPIDTO siteApiDto)
        {
            if (!string.IsNullOrEmpty(siteApiDto.AddressLine1)
                || siteApiDto.AddressTypeId > 0
                || siteApiDto.CountryId > 0
                || siteApiDto.PinCode > 0)
            {
                Address address = new Address(0,
                    siteApiDto.AddressLine1,
                    siteApiDto.AddressTypeId ?? 0,
                    siteApiDto.CountryId ?? 0,
                    siteApiDto.PinCode ?? 0,
                    siteId
                );
                _addressRepository.InsertOrUpdateAddress(address);
            }
        }

        //    // ============================================================
        //    // METHOD TIMING HELPER - RETURN VALUE
        //    // ============================================================

        //    private T ExecuteAndLog<T>(
        //        string methodName,
        //        Func<T> action)
        //    {
        //        Stopwatch methodTimer = Stopwatch.StartNew();

        //        try
        //        {
        //            return action();
        //        }
        //        finally
        //        {
        //            methodTimer.Stop();

        //            _logger.LogInformation(
        //                "\nMethod: {Method} | Time: {ElapsedMs} ms",
        //                methodName,
        //                methodTimer.ElapsedMilliseconds);
        //        }
        //    }

        //    // ============================================================
        //    // METHOD TIMING HELPER - VOID METHOD
        //    // ============================================================

        //    private void ExecuteAndLog(
        //        string methodName,
        //        Action action)
        //    {
        //        Stopwatch methodTimer = Stopwatch.StartNew();

        //        try
        //        {
        //            action();
        //        }
        //        finally
        //        {
        //            methodTimer.Stop();

        //            _logger.LogInformation(
        //                "\nMethod: {Method} | Time: {ElapsedMs} ms",
        //                methodName,
        //                methodTimer.ElapsedMilliseconds);
        //        }
        //    }

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
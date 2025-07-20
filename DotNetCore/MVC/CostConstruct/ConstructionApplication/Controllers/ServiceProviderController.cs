using AutoMapper;
using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Core.DataModels.ServiceTypes;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Transactions;
using ConstructionApplication.Repository.AdoDotNet;
using Microsoft.Extensions.DependencyInjection;

namespace ConstructionApplication.Controllers
{
    public class ServiceProviderController : BaseController
    {
        private IConfiguration _config;

        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IServiceTypeRepository _serviceTypeRepository;
        IAddressRepository _addressRepository;
        IServiceProviderRepository _serviceProviderRepository;
        IDailyAttendanceRepository _dailyAttendanceRepository;
        IMapper _imapper;
        private object iConfig;
        private readonly IWebHostEnvironment _env;

        public ServiceProviderController(IConfiguration iConfig,
            IServiceProviderRepository serviceProviderRepository,
            IAddressRepository addressRepository,
            ICountryRepository countryRepository, IServiceTypeRepository servicetypeRepository,
            IAddressTypeRepository addressTypeRepository, IWebHostEnvironment env, 
            IDailyAttendanceRepository dailyAttendanceRepository, 
            ISiteRepository siteRepository) : base(siteRepository)
        {
            _config = iConfig;
            _serviceProviderRepository = serviceProviderRepository;
            _serviceTypeRepository = servicetypeRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _addressTypeRepository = addressTypeRepository;
            _dailyAttendanceRepository = dailyAttendanceRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceProviderVm, Core.DataModels.ServiceProviders.ServiceProvider>();
                cfg.CreateMap<Core.DataModels.ServiceProviders.ServiceProvider, ServiceProviderVm>();
            });

            _imapper = configuration.CreateMapper();
            _env = env;
        }

        [SessionCheck]
        public IActionResult Index(int? serviceTypeId, int? id)
        {
            List<Core.DataModels.ServiceProviders.ServiceProvider> serviceProviders = _serviceProviderRepository.GetAll(serviceTypeId, id);
            List<ServiceProviderVm> serviceProviderVm = _imapper.Map<List<Core.DataModels.ServiceProviders.ServiceProvider>, List<ServiceProviderVm>>(serviceProviders);
            ViewBag.ServiceProviderCount = serviceProviderVm.Count;
            return View(serviceProviderVm);
        }

        [SessionCheck]
        [HttpGet]
        public IActionResult Add()
        {
            DropDownSelectList();

            return View();
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Add(ServiceProviderVm serviceProviderVm)
        {
            ModelState.Clear();

            if (!ValidateServiceProviderDetails(serviceProviderVm))
            {
                DropDownSelectList();
                return View(serviceProviderVm);
            }

            string uniqueFileName = ValidateAndUploadFile(serviceProviderVm);

            Core.DataModels.ServiceProviders.ServiceProvider serviceProvider = _imapper.Map<ServiceProviderVm, Core.DataModels.ServiceProviders.ServiceProvider>(serviceProviderVm);
            serviceProvider.ImageName = uniqueFileName;
            serviceProvider.ServiceProviderId = _serviceProviderRepository.Add(serviceProvider);
            if (serviceProvider.ServiceProviderId > 0)
            {
                AddAddressIfPresent(serviceProvider.ServiceProviderId, serviceProviderVm);
                TempData["AddSuccessMessage"] = "Your ServiceProvider Data Added successfully.";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View(serviceProviderVm);
        }

        [SessionCheck]
        public IActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Core.DataModels.ServiceProviders.ServiceProvider> serviceProviders = _serviceProviderRepository.GetAll(null, id);
            if (serviceProviders.Count == 0)
            {
                return NotFound();
            }

            ServiceProviderVm serviceProviderVm = _imapper.Map<ServiceProviderVm>(serviceProviders.First());

            DropDownSelectList();

            return View(serviceProviderVm);
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Update(ServiceProviderVm serviceProviderVm)
        {
            ModelState.Clear();

            if (!ValidateServiceProviderDetails(serviceProviderVm))
            {
                DropDownSelectList();
                return View("Edit", serviceProviderVm);
            }

            Core.DataModels.ServiceProviders.ServiceProvider serviceProvider = _imapper.Map<ServiceProviderVm, Core.DataModels.ServiceProviders.ServiceProvider>(serviceProviderVm);
            int affectedRowCount = _serviceProviderRepository.Update(serviceProvider);

            if (affectedRowCount > 0)
            {
                AddAddressIfPresent(serviceProvider.ServiceProviderId, serviceProviderVm);
                TempData["UpdateSuccessMessage"] = "Your Data updated successfully.";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View("Edit", serviceProviderVm);
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Delete(int serviceProviderId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _addressRepository.Delete(serviceProviderId, null);

                    _dailyAttendanceRepository.Delete(serviceProviderId);

                    _serviceProviderRepository.Delete(serviceProviderId);

                    transaction.Complete();

                    TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the Service Provider: " + ex.Message);
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        private bool ValidateServiceProviderDetails(ServiceProviderVm serviceProviderVm)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(serviceProviderVm.ServiceProviderName))
            {
                ViewBag.errorMessage = "Service Provider Name is required.";
                return false;
            }

            if (serviceProviderVm.ServiceProviderName.Length > 15 || serviceProviderVm.ServiceProviderName.Length < 4 ||
                !Regex.IsMatch(serviceProviderVm.ServiceProviderName, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.errorMessage = "Service Provider Name must be between 4 to 15 characters and contain only alphabets.";
                return false;
            }

            if (serviceProviderVm.DOB == null || serviceProviderVm.DOB > DateTime.Now)
            {
                ViewBag.errorMessage = "Date Of Birth cannot be null or in the future.";
                return false;
            }

            if (string.IsNullOrEmpty(serviceProviderVm.MobileNumber) || serviceProviderVm.MobileNumber.Length != 10 ||
                !Regex.IsMatch(serviceProviderVm.MobileNumber, @"^\d{10}$"))
            {
                ViewBag.errorMessage = "Mobile Number must be numeric and exactly 10 digits long.";
                return false;
            }

            if (string.IsNullOrEmpty(serviceProviderVm.ReferredBy))
            {
                ViewBag.errorMessage = "Referred name is required.";
                return false;
            }

            if (serviceProviderVm.ReferredBy.Length > 15 || serviceProviderVm.ReferredBy.Length < 3 ||
                !Regex.IsMatch(serviceProviderVm.ReferredBy, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.errorMessage = "Referred name must be between 3 to 15 characters and contain only alphabets.";
                return false;
            }

            if (serviceProviderVm.ServiceTypeId == 0)
            {
                ViewBag.errorMessage = "Job Category is required.";
                return false;
            }

            return true;
        }

        private string ValidateAndUploadFile(ServiceProviderVm serviceProviderVm)
        {
            if (serviceProviderVm.ImageFile == null || serviceProviderVm.ImageFile.Length == 0)
            {
                return null;
            }

            var fileExtension = Path.GetExtension(serviceProviderVm.ImageFile.FileName).ToLower();
            string extensions = _config.GetValue<string>("ApplicationSettings:fileExtension");
            string[] allowedExtensions = extensions.Split(',');

            if (!allowedExtensions.Contains(fileExtension))
            {
                ViewBag.errorMessage = $"Only the following file types are allowed: {string.Join(", ", allowedExtensions)}.";
                return null;
            }

            int maxFileSizeInBytes = _config.GetValue<int>("ApplicationSettings:maxFileSizeInBytes");
            if (serviceProviderVm.ImageFile.Length > maxFileSizeInBytes)
            {
                ViewBag.errorMessage = $"ImageFile size must not exceed {maxFileSizeInBytes} bytes.";
                return null;
            }

            string dir = Path.Combine(_env.WebRootPath, "UploadedImage");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string uniqueFileName = GetUniqueFileName(serviceProviderVm.ImageFile.FileName);
            string filePath = Path.Combine(dir, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                serviceProviderVm.ImageFile.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        private void AddAddressIfPresent(int serviceProviderId, ServiceProviderVm serviceProviderVm)
        {
            if (!string.IsNullOrEmpty(serviceProviderVm.AddressLine1) ||
               (serviceProviderVm.AddressTypeId.HasValue && serviceProviderVm.AddressTypeId > 0) ||
               (serviceProviderVm.CountryId.HasValue && serviceProviderVm.CountryId > 0) ||
               (serviceProviderVm.PinCode.HasValue && serviceProviderVm.PinCode > 0))
            {
                Address address = new Address(
                    serviceProviderId,
                    serviceProviderVm.AddressLine1,
                    serviceProviderVm.AddressTypeId ?? 0,
                    serviceProviderVm.CountryId ?? 0,
                    serviceProviderVm.PinCode ?? 0,
                    serviceProviderVm.SiteId
                );
                _addressRepository.InsertOrUpdateAddress(address);
            }
            else
            {
                _addressRepository.Delete(serviceProviderId, null);
            }
        }

        private void DropDownSelectList()
        {
            List<ServiceType> ServiceTypes = _serviceTypeRepository.GetAll();
            ViewBag.ServiceType = new SelectList(ServiceTypes, "Id", "Name");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");
        }
    }
}

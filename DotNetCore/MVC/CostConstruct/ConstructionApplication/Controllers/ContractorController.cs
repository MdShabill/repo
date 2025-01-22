using AutoMapper;
using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels.ContractorVm;
using ConstructionApplication.ViewModels.MaterialPurchaseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Transactions;

namespace ConstructionApplication.Controllers
{
    public class ContractorController : Controller
    {
        private IConfiguration _config;

        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IJobCategoryRepository _jobCategoryRepository;
        IAddressRepository _addressRepository;
        IContractorRepository _contractorRepository;
        IDailyAttendanceRepository _dailyAttendanceRepository;
        IMapper _imapper;
        private object iConfig;
        private readonly IWebHostEnvironment _env;

        public ContractorController(IConfiguration iConfig,
            IContractorRepository contractorRepository,  
            IAddressRepository addressRepository,
                                  ICountryRepository countryRepository, IJobCategoryRepository jobCategoryRepository,
                                  IAddressTypeRepository addressTypeRepository, IWebHostEnvironment env, 
                                  IDailyAttendanceRepository dailyAttendanceRepository)
        {
            _config = iConfig;
            _contractorRepository = contractorRepository;
            _jobCategoryRepository = jobCategoryRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _addressTypeRepository = addressTypeRepository;
            _dailyAttendanceRepository = dailyAttendanceRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContractorVm, Contractor>();
                cfg.CreateMap<Contractor, ContractorVm>();
            });

            _imapper = configuration.CreateMapper();
            _env = env;
        }

        public IActionResult Index(int? jobCategoryId, int? id)
        {
            List<Contractor> contractors = _contractorRepository.GetAll(jobCategoryId, id);
            List<ContractorVm> contractorVm = _imapper.Map<List<Contractor>, List<ContractorVm>>(contractors);
            return View(contractorVm);
        }

        [HttpGet]
        public IActionResult Add()
        {
            DropDownSelectList();

            return View();
        }

        [HttpPost]
        public IActionResult Add(ContractorVm contractorVm)
        {
            ModelState.Clear();

            if (!ValidateContractorDetails(contractorVm))
            {
                DropDownSelectList();
                return View(contractorVm);
            }

            if (contractorVm.ImageFile != null)
            {
                var fileExtension = Path.GetExtension(contractorVm.ImageFile.FileName).ToLower();

                string extensions = _config.GetValue<string>("ApplicationSettings:fileExtension");
                string[] allowedExtensions = extensions.Split(',');
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ViewBag.errorMessage = "Only .jpg, .jpeg, and .png files are allowed.";
                    DropDownSelectList();
                    return View(contractorVm);
                }

                int maxFileSizeInBytes = _config.GetValue<int>("ApplicationSettings:maxFileSizeInBytes");
                if (contractorVm.ImageFile.Length > maxFileSizeInBytes)
                {
                    ViewBag.errorMessage = $"ImageFile size must not exceed {maxFileSizeInBytes} bytes";
                    DropDownSelectList();
                    return View(contractorVm);
                }
            }

            string uniqueFileName = null;
            if (contractorVm.ImageFile != null && contractorVm.ImageFile.Length > 0)
            {
                string dir = Path.Combine(_env.WebRootPath, "UploadedImage");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                uniqueFileName = GetUniqueFileName(contractorVm.ImageFile.FileName);
                string filePath = Path.Combine(dir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    contractorVm.ImageFile.CopyTo(fileStream);
                }
            }

            Contractor contractor = _imapper.Map<ContractorVm, Contractor>(contractorVm);
            contractor.ImageName = uniqueFileName;
            contractor.ContractorId = _contractorRepository.Add(contractor);
            if(contractor.ContractorId > 0) 
            {
                if (contractorVm.AddressTypeId != null || contractorVm.CountryId != null ||
                    !string.IsNullOrEmpty(contractorVm.AddressLine1) || contractorVm.PinCode != null)
                {
                    Address address = new Address(
                        contractor.ContractorId,
                        contractorVm.AddressLine1,
                        contractorVm.AddressTypeId ?? 0,
                        contractorVm.CountryId ?? 0,
                        contractorVm.PinCode ?? 0
                    );
                    _addressRepository.Add(address);
                }
                TempData["AddSuccessMessage"] = "Your Contractor Data Added successfully.";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View(contractorVm);
        }

        public IActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Contractor> contractors = _contractorRepository.GetAll(null, id);
            if (contractors.Count == 0)
            {
                return NotFound();
            }

            ContractorVm contractorVm = _imapper.Map<ContractorVm>(contractors.First());

            DropDownSelectList();

            return View(contractorVm);
        }

        [HttpPost]
        public IActionResult Update(ContractorVm contractorVm)
        {
            ModelState.Clear();

            if (!ValidateContractorDetails(contractorVm))
            {
                DropDownSelectList();
                return View(contractorVm);
            }

            Contractor contractor = _imapper.Map<ContractorVm, Contractor>(contractorVm);
            int affectedRowCount = _contractorRepository.Update(contractor);

            if (affectedRowCount > 0)
            {
                if (contractorVm.AddressTypeId != null || contractorVm.CountryId != null ||
                    !string.IsNullOrEmpty(contractorVm.AddressLine1) || contractorVm.PinCode != null)
                {
                    Address address = new()
                    {
                        ContractorId = contractor.ContractorId,
                        AddressLine1 = contractorVm.AddressLine1,
                        AddressTypeId = contractorVm.AddressTypeId ?? 0,
                        CountryId = contractorVm.CountryId ?? 0,
                        PinCode = contractorVm.PinCode ?? 0
                    };
                    _addressRepository.InsertOrUpdateAddress(address);
                }
                TempData["UpdateSuccessMessage"] = "Your Data updated successfully.";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View(contractorVm);
        }

        [HttpPost]
        public IActionResult Delete(int contractorId)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _addressRepository.Delete(contractorId);

                    _dailyAttendanceRepository.Delete(contractorId);

                    _contractorRepository.Delete(contractorId);

                    transaction.Complete();

                    TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the contractor: " + ex.Message);
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

        private bool ValidateContractorDetails(ContractorVm contractorVm)
        {
            if (string.IsNullOrEmpty(contractorVm.ContractorName))
            {
                ViewBag.errorMessage = "Contractor Name is required.";
                return false;
            }

            if (contractorVm.ContractorName.Length > 15 || contractorVm.ContractorName.Length < 4 ||
                !Regex.IsMatch(contractorVm.ContractorName, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.errorMessage = "Contractor Name must be between 4 to 15 characters and contain only alphabets.";
                return false;
            }

            if (contractorVm.DOB == null || contractorVm.DOB > DateTime.Now)
            {
                ViewBag.errorMessage = "Date Of Birth cannot be null or in the future.";
                return false;
            }

            if (string.IsNullOrEmpty(contractorVm.MobileNumber) || contractorVm.MobileNumber.Length != 10 ||
                !Regex.IsMatch(contractorVm.MobileNumber, @"^\d{10}$"))
            {
                ViewBag.errorMessage = "Mobile Number must be numeric and exactly 10 digits long.";
                return false;
            }

            if (string.IsNullOrEmpty(contractorVm.ReferredBy))
            {
                ViewBag.errorMessage = "Referred name is required.";
                return false;
            }

            if (contractorVm.ReferredBy.Length > 15 || contractorVm.ReferredBy.Length < 3 ||
                !Regex.IsMatch(contractorVm.ReferredBy, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.errorMessage = "Referred name must be between 3 to 15 characters and contain only alphabets.";
                return false;
            }

            if (contractorVm.JobCategoryId == 0)
            {
                ViewBag.errorMessage = "Job Category is required.";
                return false;
            }

            return true;
        }

        private void DropDownSelectList()
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");
        }
    }
}

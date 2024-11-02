using AutoMapper;
using ConstructionApplication.DataModels.Address;
using ConstructionApplication.DataModels.AddressType;
using ConstructionApplication.DataModels.Brands;
using ConstructionApplication.DataModels.Contractor;
using ConstructionApplication.DataModels.Country;
using ConstructionApplication.DataModels.JobCategory;
using ConstructionApplication.DataModels.Material;
using ConstructionApplication.DataModels.Suppliers;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.ContractorVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;

namespace ConstructionApplication.Controllers
{
    public class ContractorController : Controller
    {
        IAddressTypeRepository _addressTypeRepository;
        ICountryRepository _countryRepository;
        IJobCategoryRepository _jobCategoryRepository;
        IAddressRepository _addressRepository;
        IContractorRepository _contractorRepository;
        IDailyAttendanceRepository _dailyAttendanceRepository;
        IMapper _imapper;
        private readonly IWebHostEnvironment _env;

        public ContractorController(IContractorRepository contractorRepository,  IAddressRepository addressRepository,
                                  ICountryRepository countryRepository, IJobCategoryRepository jobCategoryRepository,
                                  IAddressTypeRepository addressTypeRepository, IWebHostEnvironment env, 
                                  IDailyAttendanceRepository dailyAttendanceRepository)
        {
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
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ContractorVm contractorVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            const int maxFileSizeInMB = 2;
            const int maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;

            if (contractorVm.ImageFile != null)
            {
                var fileExtension = Path.GetExtension(contractorVm.ImageFile.FileName).ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    ViewBag.errorMessage = "ImageFile Only .jpg, .jpeg, and .png files are allowed.";
                    return View(contractorVm);
                }

                if (contractorVm.ImageFile.Length > maxFileSizeInBytes)
                {
                    ViewBag.errorMessage = "ImageFile size must not exceed 2 MB.";
                    return View(contractorVm);
                }
            }


            using (TransactionScope transactionScope = new())
            {
                try
                {
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
                            Address address = new()
                            {
                                ContractorId = contractor.ContractorId,
                                AddressLine1 = contractorVm.AddressLine1,
                                AddressTypeId = contractorVm.AddressTypeId ?? 0,
                                CountryId = contractorVm.CountryId ?? 0,
                                PinCode = contractorVm.PinCode ?? 0
                            };
                            _addressRepository.Add(address);
                            TempData["AddSuccessMessage"] = "Your Data Added successfully.";
                            return RedirectToAction("Index");
                        }
                        transactionScope.Complete();
                    }
                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();
                }
            }
            return View();
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

            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name", contractorVm.JobCategoryId);

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name", contractorVm.AddressTypeId);

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name", contractorVm.CountryId);

            return View(contractorVm);
        }

        [HttpPost]
        public IActionResult Update(ContractorVm contractorVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            List<AddressType> addressTypes = _addressTypeRepository.GetAll();
            ViewBag.AddressTypes = new SelectList(addressTypes, "Id", "Name");

            List<Country> countries = _countryRepository.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            using (TransactionScope transactionScope = new())
            {
                try
                {
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
                            _addressRepository.Update(address);
                        }
                        transactionScope.Complete();
                        TempData["UpdateSuccessMessage"] = "Your Data updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
                catch (TransactionException ex)
                {
                    transactionScope.Dispose();
                    ModelState.AddModelError("", "An error occurred while updating contractor data.");
                }
            }
            return View();
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
    }
}

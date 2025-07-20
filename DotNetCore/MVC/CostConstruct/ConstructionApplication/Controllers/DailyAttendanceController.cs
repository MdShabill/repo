using AutoMapper;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Core.DataModels.ServiceTypes;
using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace ConstructionApplication.Controllers
{
    public class DailyAttendanceController : BaseController
    {
        IDailyAttendanceRepository _dailyAttendanceRepository;
        ICostMasterRepository _costMasterRepository;
        IServiceTypeRepository _serviceTypeRepository;
        IServiceProviderRepository _serviceProviderRepository;
        ISiteRepository _siteRepository;
        IMapper _imapper;

        public DailyAttendanceController(IDailyAttendanceRepository dailyAttendanceRepository, 
                                         ICostMasterRepository costMasterRepository,
                                         IServiceTypeRepository serviceTypeRepository,
                                         IServiceProviderRepository serviceProviderRepository,
                                         ISiteRepository siteRepository) : base(siteRepository)
        {
            _dailyAttendanceRepository = dailyAttendanceRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyAttendanceVm, DailyAttendance>();
                cfg.CreateMap<DailyAttendance, DailyAttendanceVm>();
            });

            _imapper = configuration.CreateMapper();
            _costMasterRepository = costMasterRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _serviceProviderRepository = serviceProviderRepository;
            _siteRepository = siteRepository;
        }

        [SessionCheck]
        public IActionResult Index(DateTime? DateFrom, DateTime? DateTo)
        {
            //DateTime currentDate = DateTime.Now;

            //// If both DateFrom and DateTo are chosen, make sure DateFrom is not greater than DateTo.
            //// This makes sure the 'From' date comes before or is the same as the 'To' date.
            //if (DateFrom != null && DateTo != null && DateFrom > DateTo)
            //{
            //    ViewBag.errorMessage = "FROM DATE cannot be greater than TO DATE ";
            //}

            //// Check if either DateFrom or DateTo is in the future.
            //// This makes sure that both the 'From' date and 'To' date are not after today.
            //if (DateFrom > currentDate || DateTo > currentDate)
            //{
            //    ViewBag.errorMessage = "The FROM DATE and TO DATE cannot be in the future ";
            //}

            List<DailyAttendance> dailyAttendances = _dailyAttendanceRepository.GetAll(SiteId, DateFrom, DateTo);
            List<DailyAttendanceVm> dailyAttendanceVm = _imapper.Map<List<DailyAttendance>, List<DailyAttendanceVm>>(dailyAttendances);
            ViewBag.DateFrom = DateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = DateTo?.ToString("yyyy-MM-dd");
            return View(dailyAttendanceVm);
        }

        [SessionCheck]
        public IActionResult Add()
        {
            DropDownSelectList();

            return View();
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Add(DailyAttendanceVm dailyAttendanceVm)
        {
            ModelState.Clear();
            string validationMessage = ValidateDailyAttendance(dailyAttendanceVm);
            if (validationMessage != null)
            {
                ViewBag.errorMessage = validationMessage;
                DropDownSelectList();
                return View(dailyAttendanceVm);
            }

            int? siteId = HttpContext.Session.GetInt32("SelectedSiteId");
            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

            dailyAttendance.SiteId = siteId.Value;

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(dailyAttendanceVm.ServiceTypeId);

            if (costMaster != null)
            {
                dailyAttendance.TotalAmount = dailyAttendance.TotalWorker * costMaster.Cost;

                dailyAttendance.Id = _dailyAttendanceRepository.Create(dailyAttendance);
                if (dailyAttendance.Id > 0)
                {
                    TempData["SuccessMessage"] = "Add New Daily Attendance Successful";
                }
            }
            else
            {
                ViewBag.errorMessage = "No active CostMaster record found ";
                DropDownSelectList();
                return View(dailyAttendanceVm);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddUsingAjax()
        {
            DropDownSelectList();
            return View();
        }

        [HttpGet]
        public IActionResult GetCostByServiceType(int ServiceTypeId = 0)
        {
            if (ServiceTypeId > 0)

            {

                CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(ServiceTypeId);
                List<Core.DataModels.ServiceProviders.ServiceProvider> serviceProviders = _serviceProviderRepository.GetAll(ServiceTypeId, null)
                .Where(c => c.ServiceTypeId == ServiceTypeId)
                .ToList();

                return new JsonResult(new 
                {
                    cost = costMaster?.Cost ?? 0,
                    serviceProviders = serviceProviders
                });

            }
            return base.Json(new { cost = 0, serviceProviders = new List<Core.DataModels.ServiceProviders.ServiceProvider>() });
        }

        [HttpPost]
        public IActionResult AddUsingAjax(DailyAttendanceVm dailyAttendanceVm)
        {
            ModelState.Clear();

            string validationMessage = ValidateDailyAttendance(dailyAttendanceVm);
            if (validationMessage != null)
            {
                ViewBag.errorMessage = validationMessage;
                DropDownSelectList();
                return View(dailyAttendanceVm);
            }

            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(dailyAttendanceVm.ServiceTypeId);

            if (costMaster != null)
            {
                dailyAttendance.Id = _dailyAttendanceRepository.Create(dailyAttendance);
                if (dailyAttendance.Id > 0)
                {
                    TempData["SuccessMessage"] = "Add New Daily Attendance Successful";
                }
            }
            else
            {
                ViewBag.errorMessage = "No active CostMaster record found ";
                DropDownSelectList();
                return View(dailyAttendanceVm);
            }
            return RedirectToAction("Index");
        }

        private string ValidateDailyAttendance(DailyAttendanceVm dailyAttendanceVm)
        {
            if (dailyAttendanceVm.ServiceTypeId == 0)
            {
                return "Please select a Job Category.";
            }

            if (dailyAttendanceVm.TotalWorker <= 0)
            {
                return "Please enter a valid number of Total Workers.";
            }

            if (dailyAttendanceVm.Date > DateTime.Now)
            {
                return "Date cannot be in the future.";
            }

            // This regex accepts only numeric values, special characters and alphabets are not allowed.
            string totalWorkerPattern = @"^\d+$";
            if (!Regex.IsMatch(dailyAttendanceVm.TotalWorker.ToString(), totalWorkerPattern))
            {
                return "Total Worker must be a valid positive number and cannot contain any special characters or alphabets.";
            }

            return null;
        }

        private void DropDownSelectList()
        {
            List<ServiceType> serviceTypes = _serviceTypeRepository.GetAll();
            ViewBag.ServiceType = new SelectList(serviceTypes, "Id", "Name");

            var serviceTypeCosts = serviceTypes.Select(serviceTypes => new
            {
                ServiceTypeId = serviceTypes.Id,
                ServiceTypeName = serviceTypes.Name,
                Cost = _costMasterRepository.GetActiveCostDetail(serviceTypes.Id)?.Cost ?? 0
            }).ToList();

            ViewBag.ServiceTypeCosts = serviceTypeCosts;
        }

    }
}

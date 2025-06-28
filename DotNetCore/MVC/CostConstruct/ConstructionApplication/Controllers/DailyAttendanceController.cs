using AutoMapper;
using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Core.DataModels.Site;
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
        IJobCategoryRepository _jobCategoryRepository;
        IContractorRepository _contractorRepository;
        ISiteRepository _siteRepository;
        IMapper _imapper;

        public DailyAttendanceController(IDailyAttendanceRepository dailyAttendanceRepository, 
                                         ICostMasterRepository costMasterRepository,
                                         IJobCategoryRepository jobCategoryRepository,
                                         IContractorRepository contractorRepository,
                                         ISiteRepository siteRepository)
        {
            _dailyAttendanceRepository = dailyAttendanceRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyAttendanceVm, DailyAttendance>();
                cfg.CreateMap<DailyAttendance, DailyAttendanceVm>();
            });

            _imapper = configuration.CreateMapper();
            _costMasterRepository = costMasterRepository;
            _jobCategoryRepository = jobCategoryRepository;
            _contractorRepository = contractorRepository;
            _siteRepository = siteRepository;
        }

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

            int? userId = ValidateUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            int? siteId = ValidateSelectedSiteId();
            if (siteId == null || siteId <= 0)
            {
                return RedirectToAction("Index", "Site");
            }

            List<DailyAttendance> dailyAttendances = _dailyAttendanceRepository.GetAll(siteId.Value, DateFrom, DateTo);
            List<DailyAttendanceVm> dailyAttendanceVm = _imapper.Map<List<DailyAttendance>, List<DailyAttendanceVm>>(dailyAttendances);
            ViewBag.DateFrom = DateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = DateTo?.ToString("yyyy-MM-dd");
            return View(dailyAttendanceVm);
        }

        public IActionResult Add()
        {
            int? siteId = ValidateSelectedSiteId();
            if (siteId == null || siteId <= 0)
            {
                return RedirectToAction("Index", "Site");
            }

            DropDownSelectList();

            return View();
        }

        [HttpPost]
        public IActionResult Add(DailyAttendanceVm dailyAttendanceVm)
        {
            ModelState.Clear();

            int? siteId = ValidateSelectedSiteId();
            if (siteId == null || siteId <= 0)
            {
                return RedirectToAction("Index", "Site");
            }

            string validationMessage = ValidateDailyAttendance(dailyAttendanceVm);
            if (validationMessage != null)
            {
                ViewBag.errorMessage = validationMessage;
                DropDownSelectList();
                return View(dailyAttendanceVm);
            }

            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

            dailyAttendance.SiteId = siteId.Value;

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(dailyAttendanceVm.JobCategoryId);

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
        public IActionResult GetDataByJobCategoryId(int jobCategoryId = 0)
        {
            if (jobCategoryId > 0)

            {

                CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(jobCategoryId);
                List<Contractor> contractors = _contractorRepository.GetAll(jobCategoryId, null)
                .Where(c => c.JobCategoryId == jobCategoryId)
                .ToList();

                return new JsonResult(new 
                {
                    cost = costMaster?.Cost ?? 0,
                    contractors = contractors
                });

            }
            return Json(new { cost = 0, contractors = new List<Contractor>() });
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

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(dailyAttendanceVm.JobCategoryId);

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
            if (dailyAttendanceVm.JobCategoryId == 0)
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
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            var jobCategoryCosts = jobCategories.Select(jobCategory => new
            {
                JobCategoryId = jobCategory.Id,
                JobCategoryName = jobCategory.Name,
                Cost = _costMasterRepository.GetActiveCostDetail(jobCategory.Id)?.Cost ?? 0
            }).ToList();

            ViewBag.JobCategoryCosts = jobCategoryCosts;
        }

    }

}

using AutoMapper;
using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels.CostMasterVm;
using ConstructionApplication.ViewModels.DailyAttendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionApplication.Controllers
{
    public class DailyAttendanceController : Controller
    {
        IDailyAttendanceRepository _dailyAttendanceRepository;
        ICostMasterRepository _costMasterRepository;
        IJobCategoryRepository _jobCategoryRepository;
        IContractorRepository _contractorRepository;
        IMapper _imapper;

        public DailyAttendanceController(IDailyAttendanceRepository dailyAttendanceRepository, 
                                         ICostMasterRepository costMasterRepository,
                                         IJobCategoryRepository jobCategoryRepository,
                                         IContractorRepository contractorRepository)
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

            List<DailyAttendance> dailyAttendances = _dailyAttendanceRepository.GetAll(DateFrom, DateTo);
            List<DailyAttendanceVm> dailyAttendanceVm = _imapper.Map<List<DailyAttendance>, List<DailyAttendanceVm>>(dailyAttendances);
            ViewBag.DateFrom = DateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = DateTo?.ToString("yyyy-MM-dd");
            return View(dailyAttendanceVm);
        }

        public IActionResult Add(int? jobCategoryId)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            if (jobCategories.Count > 0 && jobCategoryId == 0)
            {
                jobCategoryId = jobCategories[0].Id;
            }
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name", jobCategoryId);

            var jobCategoryCosts = jobCategories.Select(jobCategory => new
            {
                JobCategoryId = jobCategory.Id,
                JobCategoryName = jobCategory.Name,
                Cost = _costMasterRepository.GetActiveCostDetail(jobCategory.Id)?.Cost ?? 0 
            }).ToList();

            ViewBag.JobCategoryCosts = jobCategoryCosts;

            return View();
        }

        [HttpPost]
        public IActionResult Add(DailyAttendanceVm dailyAttendanceVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            if (dailyAttendanceVm.JobCategoryId == 0 || dailyAttendanceVm.TotalWorker == 0 || dailyAttendanceVm.TotalWorker <= 0)
            {
                ViewBag.errorMessage = "Please provide valid inputs. Job Category and Total Worker are required.";
                return View(dailyAttendanceVm);
            }


            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

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
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddUsingAjax()
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            return View();
        }

        [HttpGet]
        public IActionResult GetDataByJobCategoryId(int jobCategoryId = 0)
        {
            if (jobCategoryId > 0)
            {

                CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(jobCategoryId);
                List<Contractor> contractors = _contractorRepository.GetAll(jobCategoryId, null);
 
                return new JsonResult(new 
                { 
                    cost = costMaster.Cost,
                    contractors = contractors
                });

            }
            return View();
        }

        //Insert Data with the using AJAX in DailyAttendance
        [HttpPost]
        public IActionResult AddUsingAjax(DailyAttendanceVm dailyAttendanceVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            if (dailyAttendanceVm.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "Date cannot be in the future.";
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
            }
            return RedirectToAction("Index");
        }
    }
}

using AutoMapper;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.DailyAttendance;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.CostMasterVm;
using ConstructionApplication.ViewModels.DailyAttendance;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApplication.Controllers
{
    public class DailyAttendanceController : Controller
    {
        IDailyAttendanceRepository _dailyAttendanceRepository;
        ICostMasterRepository _costMasterRepository;
        IMapper _imapper;

        public DailyAttendanceController(IDailyAttendanceRepository dailyAttendanceRepository, 
                                         ICostMasterRepository costMasterRepository)
        {
            _dailyAttendanceRepository = dailyAttendanceRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyAttendanceVm, DailyAttendance>();
                cfg.CreateMap<DailyAttendance, DailyAttendanceVm>();
            });

            _imapper = configuration.CreateMapper();
            _costMasterRepository = costMasterRepository;
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
            
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(DailyAttendanceVm dailyAttendanceVm)
        {
            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail();
            
            if(costMaster != null)
            {
                dailyAttendance.MasterMasonAmount = dailyAttendance.TotalMasterMason * costMaster.MasterMasonCost;
                dailyAttendance.LabourAmount = dailyAttendance.TotalLabour * costMaster.LabourCost;
                dailyAttendance.TotalAmount = dailyAttendance.MasterMasonAmount + dailyAttendance.LabourAmount;

                HttpContext.Session.SetInt32("AttendanceId", dailyAttendance.Id);

                dailyAttendance.Id = _dailyAttendanceRepository.Create(dailyAttendance);
                if (dailyAttendance.Id > 0)
                {
                    ViewBag.successMessage = "Add New Daily Attendance Successful";
                }

            }
            else
            {
                ViewBag.errorMessage = "No active CostMaster record found ";
            }

            return View();
        }

        public IActionResult AddUsingAjax()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUsingAjax(DailyAttendanceVm dailyAttendanceVm)
        {
            DailyAttendance dailyAttendance = _imapper.Map<DailyAttendanceVm, DailyAttendance>(dailyAttendanceVm);

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail();

            if (costMaster != null)
            {
                dailyAttendance.MasterMasonAmount = dailyAttendance.TotalMasterMason * costMaster.MasterMasonCost;
                dailyAttendance.LabourAmount = dailyAttendance.TotalLabour * costMaster.LabourCost;
                dailyAttendance.TotalAmount = dailyAttendance.MasterMasonAmount + dailyAttendance.LabourAmount;

                dailyAttendance.Id = _dailyAttendanceRepository.Create(dailyAttendance);
                if (dailyAttendance.Id > 0)
                {
                    ViewBag.successMessage = "Add New Daily Attendance Successful";
                }
            }
            else
            {
                ViewBag.errorMessage = "No active CostMaster record found ";
            }
            return View();
        }
    }
}

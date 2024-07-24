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

        public IActionResult Index()
        {
            List<DailyAttendance> dailyAttendances = _dailyAttendanceRepository.GetAll();
            List<DailyAttendanceVm> dailyAttendanceVm = _imapper.Map<List<DailyAttendance>, List<DailyAttendanceVm>>(dailyAttendances);
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

using AutoMapper;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels.CostMasterVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionApplication.Controllers
{
    public class CostMasterController : Controller
    {
        ICostMasterRepository _costMasterRepository;
        IJobCategoryRepository _jobCategoryRepository;
        IMapper _imapper;

        public CostMasterController(ICostMasterRepository costMasterRepository, IJobCategoryRepository jobCategoryRepository)
        {
            _costMasterRepository = costMasterRepository;
            _jobCategoryRepository = jobCategoryRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddNewCostMasterVm, CostMaster>();
                cfg.CreateMap<CostMaster, CostMasterVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index(int? jobCategoryId)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name", jobCategoryId);

            List<CostMaster> costMasters;
            if (jobCategoryId.HasValue && jobCategoryId.Value > 0)
            {
                costMasters = _costMasterRepository.GetByJobCategory(jobCategoryId.Value);
            }
            else
            {
                costMasters = _costMasterRepository.GetByJobCategory(jobCategories.First().Id);
            }

            List<CostMasterVm> costMasterVm = _imapper.Map<List<CostMaster>, List<CostMasterVm>>(costMasters);

            return View(costMasterVm);
        }

        [HttpGet]
        public JsonResult GetActiveCost(int JobCategoryId)
        {
            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(JobCategoryId);
            CostMasterVm costMasterVm = _imapper.Map<CostMaster, CostMasterVm>(costMaster);

            return Json(costMasterVm);
        }

        public IActionResult Add()
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddNewCostMasterVm costMasterVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            if (costMasterVm.Date == null || 
                costMasterVm.Date == default(DateTime) || 
                costMasterVm.Date > DateTime.Now ||
                costMasterVm.JobCategoryId == 0 || 
                costMasterVm.Cost == null || 
                costMasterVm.Cost <= 0)
            {
                ViewBag.ErrorMessage = "Page not submitted, please enter correct values";
                return View(costMasterVm);
            }

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Create(costMaster);
            if (affectedRowCount > 0)
            {
                ViewBag.successMessage = "Add New Cost Master Successful";
            }
            return View();
        }
    }
}

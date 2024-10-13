using AutoMapper;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.JobCategory;
using ConstructionApplication.DataModels.Suppliers;
using ConstructionApplication.Repositories;
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
                cfg.CreateMap<CostMasterVm, CostMaster>();
                cfg.CreateMap<CostMaster, CostMasterVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CostMaster> costMasters = _costMasterRepository.GetAll();

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

        public IActionResult Add(int jobCategoryId)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            if (jobCategoryId > 0)
            {
                CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(jobCategoryId);
                if (costMaster != null)
                {
                    return new JsonResult(new { cost = costMaster.Cost });
                }
                return new JsonResult(new { cost = 0 });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Add(CostMasterVm costMasterVm)
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");

            CostMaster costMaster = _imapper.Map<CostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Create(costMaster);
            if (affectedRowCount > 0)
            {
                ViewBag.successMessage = "Add New Cost Master Successful";
            }
            return View();
        }
    }
}

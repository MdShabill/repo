using AutoMapper;
using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repositories;
using ConstructionApplication.Repository;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels.CostMasterVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

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
            DropDownSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddNewCostMasterVm costMasterVm)
        {
            ModelState.Clear();

            if (costMasterVm.Date == null ||
                costMasterVm.Date == default(DateTime) ||
                costMasterVm.JobCategoryId == 0)
            {
                ViewBag.ErrorMessage = "Page not submitted, please enter correct Inputs";
                DropDownSelectList();
                return View(costMasterVm);
            }

            if (costMasterVm.Cost == null ||
                costMasterVm.Cost <= 0 ||
                !Regex.IsMatch(costMasterVm.Cost.ToString(), @"^\d+$"))
            {
                ViewBag.ErrorMessage = "Cost must be a positive integer and cannot contain alphabets, decimals, or special characters.";
                DropDownSelectList();
                return View(costMasterVm);
            }

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Create(costMaster);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessage"] = "Add New Cost Master Successful";
            }
            return RedirectToAction("Index");
        }

        private void DropDownSelectList()
        {
            List<JobCategory> jobCategories = _jobCategoryRepository.GetAll();
            ViewBag.JobCategory = new SelectList(jobCategories, "Id", "Name");
        }
    }
}

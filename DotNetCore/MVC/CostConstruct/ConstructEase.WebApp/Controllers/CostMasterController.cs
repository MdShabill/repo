using AutoMapper;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.ServiceTypes;
using ConstructionApplication.Repository.Interfaces;
using ConstructEase.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.Text.RegularExpressions;

namespace ConstructEase.WebApp.Controllers
{
    public class CostMasterController : BaseController
    {
        ICostMasterRepository _costMasterRepository;
        IServiceTypeRepository _serviceTypeRepository;
        IMapper _imapper;

        public CostMasterController(ICostMasterRepository costMasterRepository,
                                    IServiceTypeRepository serviceTypeRepository,
                                    ISiteRepository siteRepository) : base(siteRepository)
        {
            _costMasterRepository = costMasterRepository;
            _serviceTypeRepository = serviceTypeRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddNewCostMasterVm, CostMaster>();
                cfg.CreateMap<CostMaster, CostMasterVm>();
                cfg.CreateMap<CostMaster, AddNewCostMasterVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [SessionCheck]
        [HttpGet]
        public IActionResult Index(int? serviceTypeId)
        {
            int siteId = SiteId;

            List<ServiceType> serviceTypes = _serviceTypeRepository.GetAll();
            ViewBag.ServiceType = new SelectList(serviceTypes, "Id", "Name", serviceTypeId);

            List<CostMaster> costMasters;
            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypeId.Value, siteId);
            }
            else
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypes.First().Id, siteId);
            }

            List<CostMasterVm> costMasterVm = _imapper.Map<List<CostMaster>, List<CostMasterVm>>(costMasters);

            return View(costMasterVm);
        }

        [HttpGet]
        public JsonResult GetActiveCost(int serviceTypeId)
        {
            int siteId = SiteId;

            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(serviceTypeId, siteId);
            CostMasterVm costMasterVm = _imapper.Map<CostMaster, CostMasterVm>(costMaster);

            return Json(costMasterVm);
        }

        [SessionCheck]
        public IActionResult Add()
        {
            DropDownSelectList();
            return View();
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Add(AddNewCostMasterVm costMasterVm)
        {
            ModelState.Clear();

            string validationMessage = ValidationDetail(costMasterVm);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                ViewBag.ErrorMessage = validationMessage;
                DropDownSelectList();
                return View(costMasterVm);
            }

            costMasterVm.SiteId = SiteId;

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Create(costMaster);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessage"] = "Add New Cost Master Successful";
            }
            return RedirectToAction("Index");
        }

        [SessionCheck]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            int siteId = SiteId;

            CostMaster costMaster = _costMasterRepository.GetById(id, siteId);
            if (costMaster == null)
            {
                TempData["ErrorMessage"] = "Cost Master record not found for the current site.";
                return RedirectToAction("Index");
            }

            AddNewCostMasterVm costMasterVm = _imapper.Map<CostMaster, AddNewCostMasterVm>(costMaster);

            DropDownSelectList();
            return View(costMasterVm);
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Update(AddNewCostMasterVm costMasterVm)
        {
            ModelState.Clear();

            string validationMessage = ValidationDetail(costMasterVm);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                ViewBag.ErrorMessage = validationMessage;
                DropDownSelectList();
                return View("Edit", costMasterVm);
            }

            costMasterVm.SiteId = SiteId;

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Update(costMaster);

            if (affectedRowCount > 0)
            {
                TempData["UpdateSuccessMessage"] = "Cost Master updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Update failed. Record may not belong to the current site.";
            }
            return RedirectToAction("Index");
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int siteId = SiteId;

            _costMasterRepository.Delete(id, siteId);
            TempData["DeleteSuccessMessage"] = "Your Data Has Been Deleted successfully.";

            return RedirectToAction("Index");
        }

        private string ValidationDetail(AddNewCostMasterVm costMasterVm)
        {
            if (costMasterVm.Date == null ||
                costMasterVm.Date == default(DateTime) ||
                costMasterVm.ServiceTypeId == 0)
            {
                return "Page not submitted, please enter correct Inputs";
            }

            if (costMasterVm.Cost == null ||
            costMasterVm.Cost <= 0 ||
            !Regex.IsMatch(costMasterVm.Cost.Value.ToString("0.##"), @"^\d+(\.\d{1,2})?$"))
            {
                return "Cost must be a positive number with up to 2 decimal places and cannot contain alphabets or special characters.";
            }

            return string.Empty;
        }

        private void DropDownSelectList()
        {
            List<ServiceType> serviceTypes = _serviceTypeRepository.GetAll();
            ViewBag.ServiceType = new SelectList(serviceTypes, "Id", "Name");
        }
    }
}
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
            });

            _imapper = configuration.CreateMapper();
        }

        [SessionCheck]
        [HttpGet]
        public IActionResult Index(int? serviceTypeId)
        {
            List<ServiceType> serviceTypes = _serviceTypeRepository.GetAll();
            ViewBag.ServiceType = new SelectList(serviceTypes, "Id", "Name", serviceTypeId);

            List<CostMaster> costMasters;
            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypeId.Value);
            }
            else
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypes.First().Id);
            }

            List<CostMasterVm> costMasterVm = _imapper.Map<List<CostMaster>, List<CostMasterVm>>(costMasters);

            return View(costMasterVm);
        }

        [HttpGet]
        public JsonResult GetActiveCost(int serviceTypeId)
        {
            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail(serviceTypeId);
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

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);
            int affectedRowCount = _costMasterRepository.Create(costMaster);
            if (affectedRowCount > 0)
            {
                TempData["SuccessMessage"] = "Add New Cost Master Successful";
            }
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
                !Regex.IsMatch(costMasterVm.Cost.ToString(), @"^\d+$"))
            {
                return "Cost must be a positive integer and cannot contain alphabets, decimals, or special characters.";
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

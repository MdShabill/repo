using AutoMapper;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.CostMasterVm;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApplication.Controllers
{
    public class CostMasterController : Controller
    {
        ICostMasterRepository _costMasterRepository;
        IMapper _imapper;

        public CostMasterController(ICostMasterRepository costMasterRepository)
        {
            _costMasterRepository = costMasterRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CostMasterVm, CostMaster>();
                cfg.CreateMap<CostMaster, CostMasterVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public JsonResult GetActiveCost()
        {
            CostMaster costMaster = _costMasterRepository.GetActiveCostDetail();
            CostMasterVm costMasterVm = _imapper.Map<CostMaster, CostMasterVm>(costMaster);
            return Json(costMasterVm);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CostMasterVm costMasterVm)
        {
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

using AutoMapper;
using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.Material;
using ConstructionApplication.DataModels.MaterialPurchase;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.CostMasterVm;
using ConstructionApplication.ViewModels.MaterialPurchaseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Metrics;

namespace ConstructionApplication.Controllers
{
    public class MaterialPurchaseController : Controller
    {
        IMaterialPurchaseRepository _materialPurchaseRepository;
        IMaterialRepository _materialRepository;
        IMapper _imapper;

        public MaterialPurchaseController(IMaterialPurchaseRepository materialPurchaseRepository, 
            IMaterialRepository materialRepository)
        {
            _materialPurchaseRepository = materialPurchaseRepository;
            _materialRepository = materialRepository;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MaterialPurchaseVm, MaterialPurchase>();
            });

            _imapper = configuration.CreateMapper();
            _materialRepository = materialRepository;
        }

        public IActionResult Add()
        {
            List<Material> materials = _materialRepository.GetAll();
            ViewBag.Materials = new SelectList(materials, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(MaterialPurchaseVm materialPurchaseVm) 
        {
            List<Material> materials = _materialRepository.GetAll();
            ViewBag.Materials = new SelectList(materials, "Id", "Name");

            MaterialPurchase materialPurchase = _imapper.Map<MaterialPurchaseVm, MaterialPurchase>(materialPurchaseVm);
            int affectedRowCount = _materialPurchaseRepository.Create(materialPurchase);
            if (affectedRowCount > 0) 
            {
                ViewBag.SuccessMessage = "Add Successfuly In Material Purchase";
            }
            return View();
        }
    }
}

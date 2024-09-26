using AutoMapper;
using ConstructionApplication.DataModels.Brands;
using ConstructionApplication.DataModels.Material;
using ConstructionApplication.DataModels.MaterialPurchase;
using ConstructionApplication.DataModels.Suppliers;
using ConstructionApplication.Repositories;
using ConstructionApplication.ViewModels.MaterialPurchaseVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionApplication.Controllers
{
    public class MaterialPurchaseController : Controller
    {
        IMaterialPurchaseRepository _materialPurchaseRepository;
        ISupplierRepository _supplierRepository;
        IMaterialRepository _materialRepository;
        IBrandRepository _brandRepository;
        IMapper _imapper;

        public MaterialPurchaseController(IMaterialPurchaseRepository materialPurchaseRepository,
                  ISupplierRepository supplierRepository, IMaterialRepository materialRepository, 
                  IBrandRepository brandRepository)
        {
            _materialPurchaseRepository = materialPurchaseRepository;
            _supplierRepository = supplierRepository;
            _materialRepository = materialRepository;
            _brandRepository = brandRepository;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MaterialPurchaseVm, MaterialPurchase>();
                cfg.CreateMap<MaterialPurchase, MaterialPurchaseVm>();
            });

            _imapper = configuration.CreateMapper();
            _materialRepository = materialRepository;
        }

        public IActionResult Index()
        {
            List<MaterialPurchase> materialPurchases = _materialPurchaseRepository.GetAll();

            List<MaterialPurchaseVm> materialPurchaseVm = _imapper.Map<List<MaterialPurchase>, List<MaterialPurchaseVm>>(materialPurchases);
            ViewBag.materialPurchaseCount = materialPurchaseVm.Count;
            return View(materialPurchaseVm);
        }

        public IActionResult Add(int Id = 0)
        {
            if(Id > 0)
            {
                Material material = _materialRepository.GetMaterialInfo(Id);

                if (material != null)
                {
                    // Return JSON result containing UnitOfMeasure and UnitPrice for AJAX calls
                    return new JsonResult(new { unitOfMeasure = material.UnitOfMeasure, unitPrice = material.UnitPrice });
                }
            }
            DropDownSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(MaterialPurchaseVm materialPurchaseVm) 
        {
            DropDownSelectList();

            MaterialPurchase materialPurchase = _imapper.Map<MaterialPurchaseVm, MaterialPurchase>(materialPurchaseVm);
            int affectedRowCount = _materialPurchaseRepository.Create(materialPurchase);
            if (affectedRowCount > 0) 
            {
                ViewBag.SuccessMessage = "Add Successfuly In Material Purchase";
            }
            return View();
        }

        private void DropDownSelectList()
        {
            List<Supplier> suppliers = _supplierRepository.GetAll();
            ViewBag.Supplier = new SelectList(suppliers, "Id", "Name");

            List<Material> materials = _materialRepository.GetAll();
            ViewBag.Materials = new SelectList(materials, "Id", "Name");

            List<Brand> brands = _brandRepository.GetAll();
            ViewBag.Brands = new SelectList(brands, "Id", "Name");
        }
    }
}

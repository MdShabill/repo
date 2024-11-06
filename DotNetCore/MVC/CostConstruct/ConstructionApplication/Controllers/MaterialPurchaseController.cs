using AutoMapper;
using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Core.DataModels.MaterialPurchase;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels.DailyAttendance;
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

        public IActionResult Index(DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId)
        {
            DropDownSelectList();

            List<MaterialPurchase> materialPurchases = _materialPurchaseRepository.GetAll(DateFrom, DateTo, MaterialId, SupplierId, BrandId);

            List<MaterialPurchaseVm> materialPurchaseVm = _imapper.Map<List<MaterialPurchase>, List<MaterialPurchaseVm>>(materialPurchases);
            ViewBag.materialPurchaseCount = materialPurchaseVm.Count;
            ViewBag.DateFrom = DateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = DateTo?.ToString("yyyy-MM-dd");
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

            if (materialPurchaseVm.Date > DateTime.Today)
            {
                ViewBag.errorMessage = "Date cannot be in the future.";
                return View(materialPurchaseVm);
            }

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

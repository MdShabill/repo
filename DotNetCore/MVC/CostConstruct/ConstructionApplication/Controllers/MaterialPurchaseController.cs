using AutoMapper;
using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Core.DataModels.MaterialPurchase;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using ConstructionApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

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

        [SessionCheck]
        public IActionResult Index(DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId)
        {
            DropDownSelectList();

            int? siteId = HttpContext.Session.GetInt32("SelectedSiteId");
            List<MaterialPurchase> materialPurchases = _materialPurchaseRepository.GetAll(Convert.ToInt32(siteId), DateFrom, DateTo, MaterialId, SupplierId, BrandId);

            List<MaterialPurchaseVm> materialPurchaseVm = _imapper.Map<List<MaterialPurchase>, List<MaterialPurchaseVm>>(materialPurchases);
            ViewBag.materialPurchaseCount = materialPurchaseVm.Count;
            ViewBag.DateFrom = DateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = DateTo?.ToString("yyyy-MM-dd");
            return View(materialPurchaseVm);
        }

        [SessionCheck]
        public IActionResult Add(int Id = 0)
        {
            if (Id > 0)
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

        [SessionCheck]
        [HttpPost]
        public IActionResult Add(MaterialPurchaseVm materialPurchaseVm)
        {
            ModelState.Clear();

            if (!ValidateMaterialPurchase(materialPurchaseVm))
            {
                DropDownSelectList();
                return View(materialPurchaseVm);
            }

            int? siteId = HttpContext.Session.GetInt32("SelectedSiteId");
            MaterialPurchase materialPurchase = _imapper.Map<MaterialPurchaseVm, MaterialPurchase>(materialPurchaseVm);
            materialPurchase.SiteId = siteId.Value;
            int affectedRowCount = _materialPurchaseRepository.Create(materialPurchase);
            if (affectedRowCount > 0) 
            {
                TempData["SuccessMessage"] = "Added successfully in Material Purchase";
                return RedirectToAction("Index");
            }
            DropDownSelectList();
            return View(materialPurchaseVm);
        }

        private bool ValidateMaterialPurchase(MaterialPurchaseVm materialPurchaseVm)
        {
            ModelState.Clear();

            if (materialPurchaseVm.MaterialId <= 0 || materialPurchaseVm.SupplierId <= 0 ||
                materialPurchaseVm.BrandId <= 0 || materialPurchaseVm.Quantity <= 0 ||
                string.IsNullOrEmpty(materialPurchaseVm.UnitOfMeasure) ||
                materialPurchaseVm.Date == default ||
                materialPurchaseVm.Date > DateTime.Now ||
                materialPurchaseVm.MaterialCost <= 0 ||
                materialPurchaseVm.DeliveryCharge < 0)
            {
                ViewBag.ErrorMessage = "Please provide valid input for all required fields.";
                return false;
            }

            if (string.IsNullOrEmpty(materialPurchaseVm.PhoneNumber) ||
                materialPurchaseVm.PhoneNumber.Length != 10 ||
                !Regex.IsMatch(materialPurchaseVm.PhoneNumber, @"^\d{10}$"))
            {
                ViewBag.ErrorMessage = "Supplier Phone Number must be numeric and exactly 10 digits long.";
                return false;
            }

            if (materialPurchaseVm.Quantity <= 0 ||
                !Regex.IsMatch(materialPurchaseVm.Quantity.ToString(), @"^\d+$"))
            {
                ViewBag.ErrorMessage = "Quantity must be a positive integer and cannot contain alphabets or special characters.";
                return false;
            }

            if (materialPurchaseVm.DeliveryCharge < 0 ||
                !Regex.IsMatch(materialPurchaseVm.DeliveryCharge.ToString(), @"^\d+(\.\d{1,2})?$"))
            {
                ViewBag.ErrorMessage = "Delivery Charge must be a non-negative decimal value.";
                return false;
            }

            return true;
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
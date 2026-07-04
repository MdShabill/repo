// Path: ConstructEase.WebApp/APIControllers/MaterialPurchaseAPIController.cs
using AutoMapper;
using ConstructionApplication.Core.DataModels.Brands;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Core.DataModels.MaterialPurchase;
using ConstructionApplication.Core.DataModels.Suppliers;
using ConstructionApplication.Repository.Interfaces;
using ConstructEase.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialPurchaseAPIController : ControllerBase
    {
        private readonly IMaterialPurchaseRepository _materialPurchaseRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _imapper;

        public MaterialPurchaseAPIController(
            IMaterialPurchaseRepository materialPurchaseRepository,
            ISupplierRepository supplierRepository,
            IMaterialRepository materialRepository,
            IBrandRepository brandRepository)
        {
            _materialPurchaseRepository = materialPurchaseRepository;
            _supplierRepository = supplierRepository;
            _materialRepository = materialRepository;
            _brandRepository = brandRepository;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MaterialPurchaseVm, MaterialPurchase>();
                cfg.CreateMap<MaterialPurchase, MaterialPurchaseVm>();
            });
            _imapper = config.CreateMapper();
        }

        private int GetSiteId()
        {
            if (Request.Headers.TryGetValue("X-Site-Id", out var val)
                && int.TryParse(val, out var id))
                return id;
            return 0;
        }

        [HttpGet]
        public IActionResult Index(
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int? materialId,
            [FromQuery] int? supplierId,
            [FromQuery] int? brandId)
        {
            int siteId = GetSiteId();
            if (siteId <= 0)
                return BadRequest(new { message = "No site selected." });

            var list = _materialPurchaseRepository
                .GetAll(siteId, dateFrom, dateTo, materialId, supplierId, brandId);

            var result = _imapper.Map<List<MaterialPurchaseVm>>(list);

            return Ok(new
            {
                items = result,
                totalCount = result.Count
            });
        }

        [HttpGet("dropdown-data")]
        public IActionResult GetDropdownData()
        {
            return Ok(new
            {
                suppliers = _supplierRepository.GetAll()
                    .Select(s => new { id = s.Id, name = s.Name }),

                materials = _materialRepository.GetAll()
                    .Select(m => new { id = m.Id, name = m.Name }),

                brands = _brandRepository.GetAll()
                    .Select(b => new { id = b.Id, name = b.Name })
            });
        }

        [HttpGet("material-info")]
        public IActionResult GetMaterialInfo([FromQuery] int materialId)
        {
            if (materialId <= 0)
                return BadRequest(new { message = "Invalid material id" });

            Material material = _materialRepository.GetMaterialInfo(materialId);
            if (material == null)
                return NotFound(new { message = "Material not found" });

            return Ok(new
            {
                unitOfMeasure = material.UnitOfMeasure,
                unitPrice = material.UnitPrice
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody] MaterialPurchaseVm vm)
        {
            int siteId = GetSiteId();
            if (siteId <= 0)
                return BadRequest(new { message = "No site selected." });

            string error = Validate(vm);
            if (error != null)
                return BadRequest(new { message = error });

            var entity = _imapper.Map<MaterialPurchase>(vm);
            entity.SiteId = siteId;

            int rows = _materialPurchaseRepository.Create(entity);
            if (rows > 0)
                return Ok(new { message = "Added successfully in Material Purchase" });

            return StatusCode(500, new { message = "Failed to insert record" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid id" });

            _materialPurchaseRepository.Delete(id);
            return Ok(new { message = "Your Data Has Been Deleted successfully." });
        }

        private string Validate(MaterialPurchaseVm vm)
        {
            if (vm.MaterialId <= 0 || vm.SupplierId <= 0 || vm.BrandId <= 0
                || vm.Quantity <= 0
                || string.IsNullOrEmpty(vm.UnitOfMeasure)
                || vm.Date == default
                || vm.Date > DateTime.Now
                || vm.MaterialCost <= 0
                || vm.DeliveryCharge < 0)
                return "Please provide valid input for all required fields.";

            if (string.IsNullOrEmpty(vm.PhoneNumber)
                || vm.PhoneNumber.Length != 10
                || !Regex.IsMatch(vm.PhoneNumber, @"^\d{10}$"))
                return "Supplier Phone Number must be numeric and exactly 10 digits long.";

            if (!Regex.IsMatch(vm.Quantity.ToString(), @"^\d+$"))
                return "Quantity must be a positive integer.";

            if (!Regex.IsMatch(vm.DeliveryCharge.ToString("0.##"), @"^\d+(\.\d{1,2})?$"))
                return "Delivery Charge must be a non-negative decimal value.";

            return null;
        }
    }
}
using AutoMapper;
using ConstructEase.WebApp.ViewModels;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.AdoDotNet;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ConstructEase.WebApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostMasterAPIController : ControllerBase
    {
        ICostMasterRepository _costMasterRepository;
        IServiceTypeRepository _serviceTypeRepository;
        private readonly ISiteRepository _siteRepository;

        IMapper _imapper;

        public CostMasterAPIController(ICostMasterRepository costMasterRepository,
                                    IServiceTypeRepository serviceTypeRepository,
                                    ISiteRepository siteRepository)
        {
            _costMasterRepository = costMasterRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _siteRepository = siteRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddNewCostMasterVm, CostMaster>();
                cfg.CreateMap<CostMaster, CostMasterVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        public IActionResult Index(int? serviceTypeId)
        {
            var serviceTypes = _serviceTypeRepository.GetAll();

            if (serviceTypes == null || !serviceTypes.Any())
                return NotFound("No Service Types Found");

            List<CostMaster> costMasters;

            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypeId.Value);
            }
            else
            {
                costMasters = _costMasterRepository.GetByServiceType(serviceTypes.First().Id);
            }

            var result = _imapper.Map<List<CostMasterVm>>(costMasters);

            return Ok(result);
        }

        [HttpGet("GetActiveCost")]
        public IActionResult GetActiveCost(int serviceTypeId)
        {
            if (serviceTypeId <= 0)
                return BadRequest("Invalid Service Type Id");

            var costMaster = _costMasterRepository.GetActiveCostDetail(serviceTypeId);

            if (costMaster == null)
                return NotFound("No Active Cost Found");

            var result = _imapper.Map<CostMasterVm>(costMaster);

            return Ok(result);
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            var serviceTypes = _serviceTypeRepository.GetAll();

            if (serviceTypes == null || !serviceTypes.Any())
                return NotFound("No Service Types Found");

            return Ok(serviceTypes);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] AddNewCostMasterVm costMasterVm)
        {
            if (costMasterVm == null)
                return BadRequest("Invalid Data");

            string validationMessage = ValidationDetail(costMasterVm);

            if (!string.IsNullOrEmpty(validationMessage))
                return BadRequest(validationMessage);

            CostMaster costMaster = _imapper.Map<AddNewCostMasterVm, CostMaster>(costMasterVm);

            int affectedRowCount = _costMasterRepository.Create(costMaster);

            if (affectedRowCount > 0)
                return Ok("Add New Cost Master Successful");

            return StatusCode(500, "Failed to insert record");
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
    }
}

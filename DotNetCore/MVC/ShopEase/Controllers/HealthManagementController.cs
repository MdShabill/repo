using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels.HealthManagement;
using ShopEase.DataModels.Product;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using ShopEase.ViewModels.HealthManagement;

namespace ShopEase.Controllers
{
    public class HealthManagementController : Controller
    {
        IHealthManagementRepository _healthManagementRepository;
        IMapper _imapper;
        public HealthManagementController(IHealthManagementRepository healthManagementRepository)
        {
            _healthManagementRepository = healthManagementRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HealthManagement, HealthManagementVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetPatientMedicineDetails(int madicalRecordId) 
        {
            List<HealthManagement> healthManagements = _healthManagementRepository.GetPatientMedicineDetailBymadicalRecordId(madicalRecordId);

            List<HealthManagementVm> healthManagementVm = _imapper.Map<List<HealthManagement>, List<HealthManagementVm>>(healthManagements);

            return View(healthManagementVm);
        }

        public IActionResult GetDoctorPrescription(int madicalRecordId)
        {
            List<HealthManagement> healthManagements = _healthManagementRepository.GetDoctorPrescriptionBymadicalRecordId(madicalRecordId);

            List<HealthManagementVm> healthManagementVm = _imapper.Map<List<HealthManagement>, List<HealthManagementVm>>(healthManagements);

            return View(healthManagementVm);
        }
    }
}

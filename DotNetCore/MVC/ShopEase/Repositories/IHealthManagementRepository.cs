using ShopEase.DataModels.HealthManagement;

namespace ShopEase.Repositories
{
    public interface IHealthManagementRepository
    {
        public List<HealthManagement> GetPatientMedicineDetailBymadicalRecordId(int madicalRecordId);
        public List<HealthManagement> GetDoctorPrescriptionBymadicalRecordId(int madicalRecordId);
    }
}

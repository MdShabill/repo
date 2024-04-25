using ShopEase.DataModels.HealthManagement;

namespace ShopEase.Repositories
{
    public interface IHealthManagementRepository
    {
        public List<HealthManagement> GetPatientMedicineDetailByIdByVesitDate(int madicalRecordId);
    }
}

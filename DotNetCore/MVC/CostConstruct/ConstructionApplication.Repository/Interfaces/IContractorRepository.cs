using ConstructionApplication.Core.DataModels.Contractor;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IContractorRepository
    {
        public List<Contractor> GetAll(int? jobCategoryId, int? id);
        public int Add(Contractor contractor);
        public int Update(Contractor contractor);
        public void Delete(int ContractorId);
    }
}

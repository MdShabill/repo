using ConstructionApplication.DataModels.Contractor;

namespace ConstructionApplication.Repositories
{
    public interface IContractorRepository
    {
        public List<Contractor> GetAllContractors(int jobCategoryId);
    }
}

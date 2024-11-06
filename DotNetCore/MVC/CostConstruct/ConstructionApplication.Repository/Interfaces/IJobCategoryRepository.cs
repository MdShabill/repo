using ConstructionApplication.Core.DataModels.JobCategory;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IJobCategoryRepository
    {
        public List<JobCategory> GetAll();
    }
}

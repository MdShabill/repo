using ConstructionApplication.Core.DataModels.JobCategory;

namespace ConstructionApplication.Repositories
{
    public interface IJobCategoryRepository
    {
        public List<JobCategory> GetAll();
    }
}

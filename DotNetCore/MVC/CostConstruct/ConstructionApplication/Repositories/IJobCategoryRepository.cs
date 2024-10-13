using ConstructionApplication.DataModels.JobCategory;

namespace ConstructionApplication.Repositories
{
    public interface IJobCategoryRepository
    {
        public List<JobCategory> GetAll();
    }
}

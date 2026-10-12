using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IServiceTypeRepository
    {
        public List<ServiceType> GetAll();
    }
}

using ConstructionApplication.Core.DataModels.ServiceTypes;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IServiceTypeRepository
    {
        public List<ServiceType> GetAll();
    }
}
